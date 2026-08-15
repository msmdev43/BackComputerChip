using computerChip.DTOs.Requests.Pedido;
using computerChip.Models;
using computerChip.Models.Enum;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace computerChip.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarritoService _carritoService;
        private readonly IProductoRepository _productoRepository;

        public PedidoService(
            IPedidoRepository pedidoRepository,
            ICarritoService carritoService,
            IProductoRepository productoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _carritoService = carritoService;
            _productoRepository = productoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Pedidos?> GetByIdAsync(int id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task<Pedidos?> GetWithFullDetailsAsync(int id)
        {
            return await _pedidoRepository.GetWithFullDetailsAsync(id);
        }

        public async Task<IEnumerable<Pedidos>> GetByUsuarioAsync(int usuarioId)
        {
            return await _pedidoRepository.GetByUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<Pedidos>> GetByEstadoAsync(EstadoPedido estado)
        {
            return await _pedidoRepository.GetByEstadoAsync(estado);
        }

        public async Task<IEnumerable<Pedidos>> GetPendingPedidosAsync()
        {
            return await _pedidoRepository.GetPendingPedidosAsync();
        }

        public async Task<IEnumerable<Pedidos>> GetRecentPedidosAsync(int days)
        {
            return await _pedidoRepository.GetRecentPedidosAsync(days);
        }

        public async Task<IEnumerable<Pedidos>> GetPedidosByDateRangeAsync(DateTime desde, DateTime hasta)
        {
            return await _pedidoRepository.GetPedidosByDateRangeAsync(desde, hasta);
        }

        public async Task<IEnumerable<Pedidos>> GetAllWithDetailsAsync()
        {
            return await _pedidoRepository.GetAllWithDetailsAsync();
        }

        public async Task<int> GetPedidosCountByEstadoAsync(EstadoPedido estado)
        {
            return await _pedidoRepository.GetPedidosCountByEstadoAsync(estado);
        }

        public async Task<int> GetTotalPedidosAsync()
        {
            return await _pedidoRepository.GetTotalPedidosAsync();
        }

        public async Task<decimal> GetTotalVentasAsync()
        {
            return await _pedidoRepository.GetTotalVentasAsync();
        }

        public async Task<decimal> GetTotalVentasByPeriodoAsync(DateTime desde, DateTime hasta)
        {
            return await _pedidoRepository.GetTotalVentasByPeriodoAsync(desde, hasta);
        }

        public async Task<decimal> GetPromedioVentaAsync()
        {
            return await _pedidoRepository.GetPromedioVentaAsync();
        }

        public async Task<decimal> GetMaxVentaAsync()
        {
            return await _pedidoRepository.GetMaxVentaAsync();
        }

        public async Task<IEnumerable<Pedidos>> GetFilteredAsync(PedidoFilterRequest filter)
        {
            return await _pedidoRepository.GetFilteredAsync(filter);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Pedidos> CreatePedidoFromCarritoAsync(int usuarioId, int metodoPagoId, int zonaEnvioId)
        {
            // Obtener carrito activo del usuario
            var carrito = await _carritoService.GetActiveCarritoWithProductsAsync(usuarioId);
            if (carrito == null || !carrito.CarritoProductos.Any())
                throw new InvalidOperationException("El carrito está vacío");

            // ✅ Verificar stock de todos los productos (bool)
            foreach (var item in carrito.CarritoProductos)
            {
                if (!await _productoRepository.HasStockAsync(item.productoId))
                    throw new InvalidOperationException($"El producto {item.Productos.nombre} no está disponible");
            }

            // Crear pedido
            var pedido = new Pedidos
            {
                UsuarioId = usuarioId,
                MetodoPagoId = metodoPagoId,
                ZonaEnvioId = zonaEnvioId,
                estado = EstadoPedido.PENDIENTE,
                total = await _carritoService.GetCarritoTotalAsync(carrito.id),
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                Items = new List<ItemPedido>()
            };

            // Crear items del pedido
            foreach (var item in carrito.CarritoProductos)
            {
                var itemPedido = new ItemPedido
                {
                    cantidad = item.cantidad,
                    subtotal = item.cantidad * item.precioUnitario,
                    ItemPedidoProductos = new List<ItemPedidoProductos>
                {
                    new ItemPedidoProductos
                    {
                        productoId = item.productoId
                    }
                }
                };

                pedido.Items.Add(itemPedido);
            }

            // Guardar pedido
            await _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            // ✅ Actualizar stock de productos a false (no disponible)
            foreach (var item in carrito.CarritoProductos)
            {
                await _productoRepository.UpdateStockAsync(item.productoId, false);
            }

            // Convertir carrito a pedido
            await _carritoService.ConvertCarritoToPedidoAsync(usuarioId);

            return pedido;
        }

        public async Task<bool> CancelPedidoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.GetWithFullDetailsAsync(pedidoId);
            if (pedido == null)
                return false;

            // ✅ Devolver stock (disponible = true)
            foreach (var item in pedido.Items)
            {
                foreach (var productoItem in item.ItemPedidoProductos)
                {
                    await _productoRepository.UpdateStockAsync(productoItem.productoId, true);
                }
            }

            return await UpdateEstadoAsync(pedidoId, EstadoPedido.CANCELADO);
        }

        public async Task<bool> UpdateEstadoAsync(int pedidoId, EstadoPedido nuevoEstado)
        {
            return await _pedidoRepository.UpdateEstadoAsync(pedidoId, nuevoEstado);
        }

        public async Task<bool> ConfirmPedidoAsync(int pedidoId)
        {
            return await UpdateEstadoAsync(pedidoId, EstadoPedido.CONFIRMADO);
        }

        public async Task<bool> EnviarPedidoAsync(int pedidoId)
        {
            return await UpdateEstadoAsync(pedidoId, EstadoPedido.ENVIADO);
        }

        public async Task<bool> EntregarPedidoAsync(int pedidoId)
        {
            return await UpdateEstadoAsync(pedidoId, EstadoPedido.ENTREGADO);
        }

        public async Task<bool> AddMetodoPagoToPedidoAsync(int pedidoId, int metodoPagoId)
        {
            try
            {
                var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
                if (pedido == null)
                    return false;

                pedido.MetodoPagoId = metodoPagoId;
                pedido.updatedAt = DateTime.Now;

                _pedidoRepository.Update(pedido);
                return await _pedidoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddZonaEnvioToPedidoAsync(int pedidoId, int zonaEnvioId)
        {
            try
            {
                var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
                if (pedido == null)
                    return false;

                pedido.ZonaEnvioId = zonaEnvioId;
                pedido.updatedAt = DateTime.Now;

                _pedidoRepository.Update(pedido);
                return await _pedidoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}
