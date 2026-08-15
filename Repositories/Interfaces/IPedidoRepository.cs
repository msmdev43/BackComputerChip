using computerChip.DTOs.Requests.Pedido;
using computerChip.Models;
using computerChip.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace computerChip.Repositories.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedidos>
    {
        Task<IEnumerable<Pedidos>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Pedidos>> GetByEstadoAsync(EstadoPedido estado);
        Task<Pedidos?> GetWithFullDetailsAsync(int pedidoId);
        Task<IEnumerable<Pedidos>> GetPendingPedidosAsync();
        Task<IEnumerable<Pedidos>> GetRecentPedidosAsync(int days);
        Task<decimal> GetTotalVentasAsync();
        Task<decimal> GetTotalVentasByPeriodoAsync(DateTime desde, DateTime hasta);
        Task<int> GetPedidosCountByEstadoAsync(EstadoPedido estado);
        Task<bool> UpdateEstadoAsync(int pedidoId, EstadoPedido nuevoEstado);
        Task<Pedidos?> CreatePedidoFromCarritoAsync(int carritoId);
        Task<IEnumerable<Pedidos>> GetAllWithDetailsAsync();
        Task<IEnumerable<Pedidos>> GetPedidosByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<decimal> GetPromedioVentaAsync();
        Task<decimal> GetMaxVentaAsync();
        Task<int> GetTotalPedidosAsync();
        Task<bool> CancelPedidoAsync(int pedidoId);
        Task<bool> ConfirmPedidoAsync(int pedidoId);

        Task<IEnumerable<Pedidos>> GetFilteredAsync(PedidoFilterRequest filter);

    }
}