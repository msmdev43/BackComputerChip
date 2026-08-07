namespace computerChip.DTOs.Requests.Oferta
{
    public class OfertaUpdateRequest
    {
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? TipoOferta { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? PrecioOriginal { get; set; }
        public decimal? PrecioOferta { get; set; }
    }
}
