namespace computerChip.DTOs.Responses.Productos
{
    public class ProductoListResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal? PrecioOferta { get; set; }
        public decimal PrecioFinal => PrecioOferta ?? Precio;
        public bool Stock { get; set; }
        public string StockText => Stock ? "Disponible" : "No disponible";
        public string? ImagenPrincipal { get; set; }
        public bool IsOnSale => PrecioOferta.HasValue && PrecioOferta < Precio;
        public string? CategoriaPrincipal { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProductoSearchResponse
    {
        public List<ProductoListResponse> Productos { get; set; } = new();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
