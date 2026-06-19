using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedidos>
    {
        Task<IEnumerable<Pedidos>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Pedidos>> GetByEstadoAsync(string estado);
        Task<Pedidos?> GetWithFullDetailsAsync(int pedidoId);
        Task<IEnumerable<Pedidos>> GetPendingPedidosAsync();
        Task<IEnumerable<Pedidos>> GetRecentPedidosAsync(int days);
        Task<decimal> GetTotalVentasAsync();
        Task<decimal> GetTotalVentasByPeriodoAsync(DateTime desde, DateTime hasta);
        Task<int> GetPedidosCountByEstadoAsync(string estado);
        Task<bool> UpdateEstadoAsync(int pedidoId, string nuevoEstado);
        Task<Pedidos?> CreatePedidoFromCarritoAsync(int carritoId);
        Task<IEnumerable<Pedidos>> GetAllWithDetailsAsync();
        Task<IEnumerable<Pedidos>> GetPedidosByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<decimal> GetPromedioVentaAsync();
        Task<decimal> GetMaxVentaAsync();
        Task<int> GetTotalPedidosAsync();
        Task<bool> CancelPedidoAsync(int pedidoId);
        Task<bool> ConfirmPedidoAsync(int pedidoId);
    }
}