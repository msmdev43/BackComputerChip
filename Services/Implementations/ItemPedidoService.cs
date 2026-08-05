using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class ItemPedidoService : IItemPedidoService
    {
        private readonly IItemPedidoRepository _itemPedidoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProductoRepository _productoRepository;

        public ItemPedidoService(
            IItemPedidoRepository itemPedidoRepository,
            IPedidoRepository pedidoRepository,
            IProductoRepository productoRepository)
        {
            _itemPedidoRepository = itemPedidoRepository;
            _pedidoRepository = pedidoRepository;
            _productoRepository = productoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<ItemPedido?> GetByIdAsync(int id)
        {
            return await _itemPedidoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoAsync(int pedidoId)
        {
            return await _itemPedidoRepository.GetByPedidoAsync(pedidoId);
        }

        public async Task<IEnumerable<ItemPedido>> GetByProductoAsync(int productoId)
        {
            return await _itemPedidoRepository.GetByProductoAsync(productoId);
        }

        public async Task<ItemPedido?> GetWithFullDetailsAsync(int id)
        {
            return await _itemPedidoRepository.GetWithFullDetailsAsync(id);
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoWithProductosAsync(int pedidoId)
        {
            return await _itemPedidoRepository.GetByPedidoWithProductosAsync(pedidoId);
        }

        public async Task<decimal> GetSubtotalByPedidoAsync(int pedidoId)
        {
            return await _itemPedidoRepository.GetSubtotalByPedidoAsync(pedidoId);
        }

        public async Task<int> GetTotalItemsByPedidoAsync(int pedidoId)
        {
            return await _itemPedidoRepository.GetTotalItemsByPedidoAsync(pedidoId);
        }

        public async Task<IEnumerable<ItemPedido>> GetRecentItemsAsync(int days)
        {
            return await _itemPedidoRepository.GetRecentItemsAsync(days);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<bool> UpdateItemPedidoAsync(int id, bool disponible)
        {
            try
            {
                var itemPedido = await _itemPedidoRepository.GetWithFullDetailsAsync(id);
                if (itemPedido == null)
                    return false;

                var productoId = itemPedido.ItemPedidoProductos.FirstOrDefault()?.productoId;
                if (!productoId.HasValue)
                    return false;

                // ✅ Actualizar disponibilidad del producto
                await _productoRepository.UpdateStockAsync(productoId.Value, disponible);

                // El item del pedido no se modifica, solo el stock del producto
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Método CORREGIDO para crear ItemPedido
        public async Task<ItemPedido> CreateItemPedidoAsync(int pedidoId, int productoId, int cantidad, decimal precioUnitario)
        {
            // Verificar que el pedido existe
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException("El pedido no existe");

            // ✅ Verificar que el producto existe y tiene stock (bool)
            if (!await _productoRepository.HasStockAsync(productoId))
                throw new InvalidOperationException("El producto no está disponible");

            var itemPedido = new ItemPedido
            {
                cantidad = cantidad,
                subtotal = cantidad * precioUnitario,
                ItemPedidoProductos = new List<ItemPedidoProductos>
            {
                new ItemPedidoProductos
                {
                    productoId = productoId
                }
            }
            };

            // Agregar item al pedido
            await _itemPedidoRepository.AddAsync(itemPedido);
            await _itemPedidoRepository.SaveChangesAsync();

            // ✅ Actualizar stock del producto a false (no disponible)
            await _productoRepository.UpdateStockAsync(productoId, false);

            // Actualizar total del pedido
            await RecalcularTotalPedidoAsync(pedidoId);

            return itemPedido;
        }

        // ✅ Método CORREGIDO para eliminar ItemPedido
        public async Task<bool> DeleteItemPedidoAsync(int id)
        {
            try
            {
                var itemPedido = await _itemPedidoRepository.GetWithFullDetailsAsync(id);
                if (itemPedido == null)
                    return false;

                // ✅ Devolver stock a disponible (true)
                var productoId = itemPedido.ItemPedidoProductos.FirstOrDefault()?.productoId;
                if (productoId.HasValue)
                {
                    await _productoRepository.UpdateStockAsync(productoId.Value, true);
                }

                _itemPedidoRepository.Remove(itemPedido);
                await _itemPedidoRepository.SaveChangesAsync();

                // Actualizar total del pedido
                var pedidoId = itemPedido.PedidosItemPedido.FirstOrDefault()?.Pedidos?.id;
                if (pedidoId.HasValue)
                {
                    await RecalcularTotalPedidoAsync(pedidoId.Value);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddProductToItemPedidoAsync(int itemPedidoId, int productoId)
        {
            try
            {
                var itemPedido = await _itemPedidoRepository.GetWithFullDetailsAsync(itemPedidoId);
                if (itemPedido == null)
                    return false;

                var producto = await _productoRepository.GetByIdAsync(productoId);
                if (producto == null)
                    return false;

                // Verificar que el producto no esté ya en el item
                if (itemPedido.ItemPedidoProductos.Any(ip => ip.productoId == productoId))
                    return true;

                itemPedido.ItemPedidoProductos.Add(new ItemPedidoProductos
                {
                    itemPedidoId = itemPedidoId,
                    productoId = productoId
                });

                _itemPedidoRepository.Update(itemPedido);
                return await _itemPedidoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveProductFromItemPedidoAsync(int itemPedidoId, int productoId)
        {
            try
            {
                var itemPedido = await _itemPedidoRepository.GetWithFullDetailsAsync(itemPedidoId);
                if (itemPedido == null)
                    return false;

                var toRemove = itemPedido.ItemPedidoProductos
                    .FirstOrDefault(ip => ip.productoId == productoId);

                if (toRemove == null)
                    return true;

                // Si es el último producto del item, eliminar el item completo
                if (itemPedido.ItemPedidoProductos.Count <= 1)
                {
                    return await DeleteItemPedidoAsync(itemPedidoId);
                }

                itemPedido.ItemPedidoProductos.Remove(toRemove);
                _itemPedidoRepository.Update(itemPedido);
                return await _itemPedidoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        // ============================================
        // MÉTODOS AUXILIARES
        // ============================================

        private async Task RecalcularTotalPedidoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                return;

            var total = await _itemPedidoRepository.GetSubtotalByPedidoAsync(pedidoId);
            pedido.total = total;
            pedido.updatedAt = DateTime.Now;

            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();
        }
    }
}