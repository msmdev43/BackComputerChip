// DTOs/Requests/MetodoPago/MetodoPagoCreateRequest.cs
namespace computerChip.DTOs.Requests.MetodoPago
{
    public class MetodoPagoCreateRequest
    {
        public string Tipo { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public bool TieneDesc { get; set; }
    }

    public class MetodoPagoUpdateRequest
    {
        public string? Tipo { get; set; }
        public decimal? Descuento { get; set; }
        public bool? TieneDesc { get; set; }
    }
}