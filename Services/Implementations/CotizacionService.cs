using computerChip.Data;
using computerChip.DTOs.Requests.Checkout;
using computerChip.DTOs.Responses;
using computerChip.Models;
using computerChip.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Services.Implementations
{
    public class CotizacionService : ICotizacionService
    {
        private readonly AppDbContext _context;

        private const int MinutosValidez = 20;
        private const int CantidadMaximaPorProducto = 99;
        private const decimal ImporteMaximo = 99999999.99m;

        public CotizacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutResponse> CrearCotizacionAsync(
            int usuarioId,
            CheckoutRequest request)
        {
            // ============================================
            // 1. VALIDACIONES GENERALES
            // ============================================

            if (usuarioId <= 0)
                throw new InvalidOperationException("Usuario no válido.");

            if (request == null)
                throw new InvalidOperationException("La solicitud es obligatoria.");

            if (request.Items == null || !request.Items.Any())
                throw new InvalidOperationException("El carrito no contiene productos.");

            if (request.Comprador == null)
                throw new InvalidOperationException("Los datos del comprador son obligatorios.");

            if (request.Entrega == null)
                throw new InvalidOperationException("Los datos de entrega son obligatorios.");

            if (request.Items.Any(i =>
                i.ProductoId <= 0 ||
                i.Cantidad < 1 ||
                i.Cantidad > CantidadMaximaPorProducto))
            {
                throw new InvalidOperationException(
                    "Los productos o las cantidades solicitadas no son válidos.");
            }

            // Evitamos que un mismo producto aparezca repetido
            // en diferentes líneas de la cotización.
            if (request.Items
                .GroupBy(i => i.ProductoId)
                .Any(g => g.Count() > 1))
            {
                throw new InvalidOperationException(
                    "La solicitud contiene productos duplicados.");
            }

            // ============================================
            // 2. VALIDAR USUARIO
            // ============================================

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.id == usuarioId && u.deletedAt == null);

            if (!usuarioExiste)
                throw new InvalidOperationException("El usuario no existe o no está disponible.");

            // ============================================
            // 3. RECUPERAR PRODUCTOS
            // ============================================

            var productoIds = request.Items
                .Select(i => i.ProductoId)
                .ToList();

            var productos = await _context.Productos
                .AsNoTracking()
                .Where(p =>
                    productoIds.Contains(p.id) &&
                    p.deletedAt == null)
                .ToListAsync();

            if (productos.Count != productoIds.Count)
                throw new InvalidOperationException(
                    "Uno o más productos no existen o ya no están disponibles.");

            var productosPorId = productos.ToDictionary(p => p.id);

            // ============================================
            // 4. VALIDAR STOCK Y CALCULAR PRECIOS
            // ============================================

            var itemsCotizacion = new List<ItemCotizacion>();
            decimal subtotal = 0m;

            foreach (var itemRequest in request.Items)
            {
                var producto = productosPorId[itemRequest.ProductoId];

                if (!producto.stock)
                {
                    throw new InvalidOperationException(
                        $"El producto '{producto.nombre}' está agotado.");
                }

                // Regla comercial acordada:
                // precioOferta si tiene valor; de lo contrario, precio.
                decimal precioUnitario = producto.precioOferta
                    ?? producto.precio;

                if (precioUnitario < 0)
                {
                    throw new InvalidOperationException(
                        $"El producto '{producto.nombre}' tiene un precio inválido.");
                }

                decimal subtotalItem;

                try
                {
                    subtotalItem = checked(precioUnitario * itemRequest.Cantidad);
                    subtotal = checked(subtotal + subtotalItem);
                }
                catch (OverflowException)
                {
                    throw new InvalidOperationException(
                        "El importe de la cotización supera el límite permitido.");
                }

                if (subtotalItem > ImporteMaximo || subtotal > ImporteMaximo)
                {
                    throw new InvalidOperationException(
                        "El importe de la cotización supera el límite permitido.");
                }

                itemsCotizacion.Add(new ItemCotizacion
                {
                    ProductoId = producto.id,
                    Nombre = producto.nombre,
                    PrecioUnitario = precioUnitario,
                    Cantidad = itemRequest.Cantidad,
                    Subtotal = subtotalItem
                });
            }


            // ============================================
            // 5. VALIDAR ENTREGA Y CALCULAR ENVÍO
            // ============================================

            var tipoEntrega = request.Entrega.Tipo?.Trim().ToLowerInvariant();

            if (tipoEntrega != "envio")
            {
                throw new InvalidOperationException(
                    "Por el momento solo está disponible el envío a domicilio.");
            }

            decimal envio = 0m;
            string? provincia = request.Entrega.Provincia?.Trim();
            string? ciudad = request.Entrega.Ciudad?.Trim();
            string? codigoPostal = request.Entrega.CodigoPostal?.Trim();
            string? direccion = request.Entrega.Direccion?.Trim();

            var advertencias = new List<string>();
            bool puedeConfirmar = true;

            if (string.IsNullOrWhiteSpace(provincia) ||
                string.IsNullOrWhiteSpace(ciudad) ||
                string.IsNullOrWhiteSpace(codigoPostal) ||
                string.IsNullOrWhiteSpace(direccion))
            {
                throw new InvalidOperationException(
                    "Para realizar un envío, provincia, ciudad, código postal y dirección son obligatorios.");
            }

            var zona = await _context.ZonasEnvio
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.codigoPostal == codigoPostal);

            if (zona == null)
            {
                puedeConfirmar = false;
                advertencias.Add(
                    "No tenemos cobertura de envío para el código postal ingresado.");
            }
            else if (
                !string.Equals(
                    zona.provincia.Trim(),
                    provincia,
                    StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(
                    zona.ciudad.Trim(),
                    ciudad,
                    StringComparison.OrdinalIgnoreCase))
            {
                puedeConfirmar = false;
                advertencias.Add(
                    "La ciudad o provincia ingresada no coincide con la zona de envío disponible.");
            }
            else
            {
                bool todosTienenEnvioGratis = productos.All(p => p.envioGratis);

                envio = todosTienenEnvioGratis
                    ? 0m
                    : zona.costo;
            }

            // ============================================
            // 6. CALCULAR TOTAL
            // ============================================

            decimal total;

            try
            {
                total = checked(subtotal + envio);
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException(
                    "El importe total supera el límite permitido.");
            }

            if (total > ImporteMaximo)
            {
                throw new InvalidOperationException(
                    "El importe total supera el límite permitido.");
            }

            // ============================================
            // 7. CONSTRUIR COTIZACIÓN
            // ============================================

            var fechaCreacion = DateTime.UtcNow;

            var cotizacion = new Cotizacion
            {
                id = Guid.NewGuid().ToString("N"),
                UsuarioId = usuarioId,

                compradorNombre = request.Comprador.Nombre.Trim(),
                compradorEmail = request.Comprador.Email.Trim(),
                compradorTelefono = request.Comprador.Telefono.Trim(),

                entregaTipo = tipoEntrega!,
                entregaProvincia = provincia,
                entregaCiudad = ciudad,
                entregaCodigoPostal = codigoPostal,
                entregaDireccion = direccion,
                ZonaEnvioId = puedeConfirmar ? zona?.id : null,

                observaciones = string.IsNullOrWhiteSpace(request.Observaciones)
                    ? null
                    : request.Observaciones.Trim(),

                subtotal = subtotal,
                envio = envio,
                total = total,

                puedeConfirmar = puedeConfirmar,
                advertencias = advertencias.Count > 0
                ? string.Join("\n", advertencias)
                : null,

                fechaCreacion = fechaCreacion,
                fechaVencimiento = fechaCreacion.AddMinutes(MinutosValidez),

                utilizada = false,

                Items = itemsCotizacion
            };

            // ============================================
            // 8. GUARDAR EN UNA TRANSACCIÓN
            // ============================================

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Cotizaciones.AddAsync(cotizacion);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            // ============================================
            // 9. CONSTRUIR RESPUESTA
            // ============================================

            return new CheckoutResponse
            {
                CotizacionId = cotizacion.id,

                Items = cotizacion.Items.Select(i => new ItemCotizacionResponse
                {
                    ProductoId = i.ProductoId,
                    Nombre = i.Nombre,
                    Cantidad = i.Cantidad,
                    Subtotal = i.Subtotal
                }).ToList(),

                Subtotal = cotizacion.subtotal,
                Envio = cotizacion.envio,
                Total = cotizacion.total,

                PuedeConfirmar = cotizacion.puedeConfirmar,
                Advertencias = advertencias
            };
        }
    }
}