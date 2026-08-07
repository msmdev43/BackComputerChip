namespace computerChip.DTOs.Requests.Oferta
{
    public class OfertaCreateRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public string TipoOferta { get; set; } = string.Empty;
        public string TipoDescuento { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioOferta { get; set; }
        public List<int> ProductosIds { get; set; } = new();
    }
}
