using computerChip.DTOs.Requests.Checkout;
using computerChip.DTOs.Responses;

namespace computerChip.Services.Interfaces
{
    public interface ICotizacionService
    {
        Task<CheckoutResponse> CrearCotizacionAsync(
            int usuarioId,
            CheckoutRequest request
        );
    }
}