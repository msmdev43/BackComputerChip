namespace computerChip.DTOs.Responses.Productos
{
    public sealed class ProductoImagenResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Orden { get; set; }
        public bool Principal { get; set; }
    }
}
