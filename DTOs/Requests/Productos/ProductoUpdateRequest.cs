namespace computerChip.DTOs.Requests.Productos
{
    public class ProductoUpdateRequest
    {
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string? Garantia { get; set; }
        public bool? Stock { get; set; }  // ✅ true = disponible, false = no disponible
        public int? EnvioGratis { get; set; }
        public string? CodigoSerie { get; set; }
    }
}
