namespace computerChip.DTOs.Requests.Productos
{
    public class ProductoSearchRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoriaId { get; set; }
        public int? MarcaId { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public bool? InStock { get; set; }  // ✅ true = solo disponibles, false = solo no disponibles
        public bool? OnSale { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? OrderBy { get; set; }
        public bool OrderDescending { get; set; } = false;
    }
}
