using computerChip.DTOs.Requests.Pedido;
using computerChip.Models;
using computerChip.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace computerChip.Services.Interfaces
{
    public interface IPedidoService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Pedidos?> GetByIdAsync(int id);
        Task<Pedidos?> GetWithFullDetailsAsync(int id);
        Task<IEnumerable<Pedidos>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Pedidos>> GetByEstadoAsync(EstadoPedido estado);
        Task<IEnumerable<Pedidos>> GetPendingPedidosAsync();
        Task<IEnumerable<Pedidos>> GetRecentPedidosAsync(int days);
        Task<IEnumerable<Pedidos>> GetPedidosByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<Pedidos>> GetAllWithDetailsAsync();
        Task<int> GetPedidosCountByEstadoAsync(EstadoPedido estado);
        Task<int> GetTotalPedidosAsync();
        Task<decimal> GetTotalVentasAsync();
        Task<decimal> GetTotalVentasByPeriodoAsync(DateTime desde, DateTime hasta);
        Task<decimal> GetPromedioVentaAsync();
        Task<decimal> GetMaxVentaAsync();

        Task<IEnumerable<Pedidos>> GetFilteredAsync(PedidoFilterRequest filter);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Pedidos> CreatePedidoFromCarritoAsync(int usuarioId, int metodoPagoId, int zonaEnvioId);
        Task<bool> UpdateEstadoAsync(int pedidoId, EstadoPedido nuevoEstado);
        Task<bool> ConfirmPedidoAsync(int pedidoId);
        Task<bool> CancelPedidoAsync(int pedidoId);
        Task<bool> EnviarPedidoAsync(int pedidoId);
        Task<bool> EntregarPedidoAsync(int pedidoId);
        Task<bool> AddMetodoPagoToPedidoAsync(int pedidoId, int metodoPagoId);
        Task<bool> AddZonaEnvioToPedidoAsync(int pedidoId, int zonaEnvioId);
    }
}