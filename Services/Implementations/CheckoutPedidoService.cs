using System.Data;
using System.Security.Cryptography;
using System.Text;
using computerChip.Data;
using computerChip.DTOs.Requests.Checkout;
using computerChip.DTOs.Responses;
using computerChip.Models;
using computerChip.Models.Enum;
using computerChip.Models.TablasIntermedias;
using computerChip.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace computerChip.Services.Implementations
{
    public class CheckoutConflictException : Exception
    {
        public CheckoutConflictException(string message) : base(message)
        {
        }
    }

    public class CheckoutPedidoService : ICheckoutPedidoService
    {
        private const int MaximoReintentosConcurrencia = 2;
        private readonly AppDbContext _context;

        public CheckoutPedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutPedidoResponse> CrearPedidoAsync(
            int usuarioId,
            CrearPedidoRequest request,
            string idempotencyKey)
        {
            if (usuarioId <= 0)
                throw new UnauthorizedAccessException("Usuario no válido.");

            if (request == null || string.IsNullOrWhiteSpace(request.CotizacionId))
                throw new CheckoutConflictException("La cotización es obligatoria.");

            if (!Guid.TryParse(idempotencyKey, out var parsedKey))
                throw new CheckoutConflictException("El encabezado Idempotency-Key debe contener un UUID válido.");

            var key = parsedKey.ToString("D");
            var cotizacionId = request.CotizacionId.Trim();

            if (cotizacionId.Length != 32 || !Guid.TryParseExact(cotizacionId, "N", out _))
                throw new CheckoutConflictException("El identificador de cotización no es válido.");

            var requestHash = CalcularHash(usuarioId, cotizacionId);

            var pedidoExistente = await BuscarPedidoPorClaveAsync(key);
            if (pedidoExistente != null)
                return ObtenerRespuestaIdempotente(pedidoExistente, usuarioId, requestHash);

            for (var intento = 0; ; intento++)
            {
                try
                {
                    return await CrearPedidoEnTransaccionAsync(
                        usuarioId, cotizacionId, key, requestHash);
                }
                catch (Exception ex) when (EsConflictoDeUnicidad(ex))
                {
                    LimpiarSeguimiento();

                    // Otra solicitud pudo haber confirmado la misma clave.
                    pedidoExistente = await BuscarPedidoPorClaveAsync(key);
                    if (pedidoExistente != null)
                        return ObtenerRespuestaIdempotente(pedidoExistente, usuarioId, requestHash);

                    // La cotización pudo haber sido consumida con otra clave.
                    var pedidoPorCotizacion = await _context.Pedidos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.CotizacionId == cotizacionId);

                    if (pedidoPorCotizacion != null)
                        throw new CheckoutConflictException("Esta cotización ya fue confirmada.");

                    throw new CheckoutConflictException(
                        "No se pudo confirmar el pedido porque se produjo un conflicto de concurrencia.");
                }
                catch (Exception ex) when (EsDeadlockOBloqueo(ex) && intento < MaximoReintentosConcurrencia)
                {
                    // MySQL revierte la transacción afectada por deadlock o timeout.
                    // Limpiamos entidades agregadas/trackeadas antes de reintentar.
                    LimpiarSeguimiento();
                    await Task.Delay(100 * (intento + 1));
                }
            }
        }

        private async Task<CheckoutPedidoResponse> CrearPedidoEnTransaccionAsync(
            int usuarioId,
            string cotizacionId,
            string key,
            string requestHash)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            // Segunda comprobación dentro de la transacción para cubrir reintentos.
            var pedidoExistente = await _context.Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdempotencyKey == key);

            if (pedidoExistente != null)
            {
                var respuesta = ObtenerRespuestaIdempotente(pedidoExistente, usuarioId, requestHash);
                await transaction.CommitAsync();
                return respuesta;
            }

            var cotizacion = await _context.Cotizaciones
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.id == cotizacionId);

            if (cotizacion == null)
                throw new CheckoutConflictException("La cotización no existe.");

            if (cotizacion.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException(
                    "No tenés permiso para confirmar esta cotización.");

            if (cotizacion.utilizada)
                throw new CheckoutConflictException("Esta cotización ya fue utilizada.");

            if (cotizacion.fechaVencimiento <= DateTime.UtcNow)
                throw new CheckoutConflictException("La cotización venció. Debés generar una nueva.");

            if (!cotizacion.puedeConfirmar)
                throw new CheckoutConflictException(
                    "La cotización no puede confirmarse por falta de cobertura de envío.");

            if (!cotizacion.ZonaEnvioId.HasValue)
                throw new CheckoutConflictException("La cotización no tiene una zona de envío válida.");

            if (cotizacion.Items == null || !cotizacion.Items.Any())
                throw new CheckoutConflictException("La cotización no contiene productos.");

            var zonaExiste = await _context.ZonasEnvio
                .AnyAsync(z => z.id == cotizacion.ZonaEnvioId.Value);

            if (!zonaExiste)
                throw new CheckoutConflictException("La zona de envío ya no se encuentra disponible.");

            var productoIds = cotizacion.Items
                .Select(i => i.ProductoId)
                .Distinct()
                .ToList();

            var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.id))
                .ToDictionaryAsync(p => p.id);

            foreach (var item in cotizacion.Items)
            {
                if (!productos.TryGetValue(item.ProductoId, out var producto)
                    || producto.deletedAt != null
                    || !producto.stock)
                {
                    throw new CheckoutConflictException(
                        $"El producto '{item.Nombre}' ya no está disponible.");
                }
            }

            var ahora = DateTime.UtcNow;
            var pedido = new Pedidos
            {
                UsuarioId = usuarioId,
                CotizacionId = cotizacion.id,
                IdempotencyKey = key,
                RequestHash = requestHash,
                MetodoPagoId = null,
                ZonaEnvioId = cotizacion.ZonaEnvioId.Value,
                OfertaId = null,
                estado = EstadoPedido.PENDIENTE,
                estadoPago = "NO_SOLICITADO",
                compradorNombre = cotizacion.compradorNombre,
                compradorEmail = cotizacion.compradorEmail,
                compradorTelefono = cotizacion.compradorTelefono,
                entregaTipo = cotizacion.entregaTipo,
                entregaProvincia = cotizacion.entregaProvincia ?? string.Empty,
                entregaCiudad = cotizacion.entregaCiudad ?? string.Empty,
                entregaCodigoPostal = cotizacion.entregaCodigoPostal ?? string.Empty,
                entregaDireccion = cotizacion.entregaDireccion ?? string.Empty,
                observaciones = cotizacion.observaciones,
                subtotal = cotizacion.subtotal,
                envio = cotizacion.envio,
                total = cotizacion.total,
                createdAt = ahora,
                updatedAt = ahora
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            foreach (var itemCotizacion in cotizacion.Items)
            {
                var itemPedido = new ItemPedido
                {
                    cantidad = itemCotizacion.Cantidad,
                    nombreProducto = itemCotizacion.Nombre,
                    precioUnitario = itemCotizacion.PrecioUnitario,
                    subtotal = itemCotizacion.Subtotal
                };

                _context.ItemsPedido.Add(itemPedido);
                await _context.SaveChangesAsync();

                _context.PedidosItemPedido.Add(new PedidosItemPedido
                {
                    pedidosId = pedido.id,
                    itemPedidoId = itemPedido.id
                });

                _context.ItemPedidoProductos.Add(new ItemPedidoProductos
                {
                    itemPedidoId = itemPedido.id,
                    productoId = itemCotizacion.ProductoId
                });
            }

            cotizacion.utilizada = true;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CrearRespuesta(pedido);
        }

        private Task<Pedidos?> BuscarPedidoPorClaveAsync(string key)
        {
            return _context.Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdempotencyKey == key);
        }

        private void LimpiarSeguimiento()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }

        private static bool EsConflictoDeUnicidad(Exception exception)
        {
            var mysqlException = ObtenerMySqlException(exception);
            return mysqlException?.Number == 1062;
        }

        private static bool EsDeadlockOBloqueo(Exception exception)
        {
            var mysqlException = ObtenerMySqlException(exception);
            return mysqlException?.Number is 1205 or 1213;
        }

        private static MySqlException? ObtenerMySqlException(Exception exception)
        {
            for (Exception? actual = exception; actual != null; actual = actual.InnerException)
            {
                if (actual is MySqlException mysqlException)
                    return mysqlException;
            }

            return null;
        }

        private static string CalcularHash(int usuarioId, string cotizacionId)
        {
            var contenido = $"{usuarioId}|{cotizacionId}";
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contenido));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private static CheckoutPedidoResponse ObtenerRespuestaIdempotente(
            Pedidos pedido,
            int usuarioId,
            string requestHash)
        {
            if (pedido.UsuarioId != usuarioId
                || !string.Equals(pedido.RequestHash, requestHash, StringComparison.Ordinal))
            {
                throw new CheckoutConflictException(
                    "La clave Idempotency-Key ya fue utilizada con otra solicitud.");
            }

            return CrearRespuesta(pedido);
        }

        private static CheckoutPedidoResponse CrearRespuesta(Pedidos pedido)
        {
            return new CheckoutPedidoResponse
            {
                Id = pedido.id,
                Numero = $"CC-{pedido.id:D6}",
                EstadoPedido = "Pendiente de disponibilidad",
                EstadoPago = "No solicitado",
                Mensaje = "Revisaremos la disponibilidad y te contactaremos."
            };
        }
    }
}
