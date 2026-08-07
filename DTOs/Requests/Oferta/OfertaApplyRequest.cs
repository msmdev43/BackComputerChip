namespace computerChip.DTOs.Requests.Oferta
{
    public class OfertaApplyRequest
    {
        public int OfertaId { get; set; }
        public List<int> ProductosIds { get; set; } = new();
    }
}
