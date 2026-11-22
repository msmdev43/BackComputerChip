namespace computerChip.DTOs.Requests.Productos
{
    public sealed class ProductoImagenManifestItem
    {
        public int? Id { get; set; }
        public string? Archivo { get; set; }
        public int Orden { get; set; }
        public bool Principal { get; set; }
    }
}
