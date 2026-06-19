using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IItemPedidoRepository : IRepository<ItemPedido>
    {
        Task<IEnumerable<ItemPedido>> GetByPedidoAsync(int pedidoId);
        Task<IEnumerable<ItemPedido>> GetByProductoAsync(int productoId);
        Task<ItemPedido?> GetWithFullDetailsAsync(int id);
        Task<IEnumerable<ItemPedido>> GetByPedidoWithProductosAsync(int pedidoId);
        Task<decimal> GetSubtotalByPedidoAsync(int pedidoId);
        Task<int> GetTotalItemsByPedidoAsync(int pedidoId);
        Task<IEnumerable<ItemPedido>> GetRecentItemsAsync(int days);
    }
}