using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IItemPedidoService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<ItemPedido?> GetByIdAsync(int id);
        Task<IEnumerable<ItemPedido>> GetByPedidoAsync(int pedidoId);
        Task<IEnumerable<ItemPedido>> GetByProductoAsync(int productoId);
        Task<ItemPedido?> GetWithFullDetailsAsync(int id);
        Task<IEnumerable<ItemPedido>> GetByPedidoWithProductosAsync(int pedidoId);
        Task<decimal> GetSubtotalByPedidoAsync(int pedidoId);
        Task<int> GetTotalItemsByPedidoAsync(int pedidoId);
        Task<IEnumerable<ItemPedido>> GetRecentItemsAsync(int days);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<ItemPedido> CreateItemPedidoAsync(int pedidoId, int productoId, int cantidad, decimal precioUnitario);
        Task<bool> UpdateItemPedidoAsync(int id, bool disponible);
        Task<bool> DeleteItemPedidoAsync(int id);
        Task<bool> AddProductToItemPedidoAsync(int itemPedidoId, int productoId);
        Task<bool> RemoveProductFromItemPedidoAsync(int itemPedidoId, int productoId);
    }
}