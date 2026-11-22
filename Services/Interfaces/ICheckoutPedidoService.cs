using computerChip.DTOs.Requests.Checkout;
using computerChip.DTOs.Responses;

namespace computerChip.Services.Interfaces
{
    public interface ICheckoutPedidoService
    {
        Task<CheckoutPedidoResponse> CrearPedidoAsync(
            int usuarioId,
            CrearPedidoRequest request,
            string idempotencyKey);
    }
}
