using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Checkout
{
    public class CrearPedidoRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string CotizacionId { get; set; } = string.Empty;
    }
}
