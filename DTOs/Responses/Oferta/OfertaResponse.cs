// DTOs/Responses/Oferta/OfertaResponse.cs
namespace computerChip.DTOs.Responses.Oferta
{
    public class OfertaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public string TipoOferta { get; set; } = string.Empty;
        public string TipoDescuento { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioOferta { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<OfertaProductoResponse> Productos { get; set; } = new();
    }

    public class OfertaProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Stock { get; set; }
        public string? ImagenPrincipal { get; set; }
    }

    public class OfertaListResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public int ProductosCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}