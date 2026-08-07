namespace computerChip.DTOs.Responses.Carrito
{
    public class CarritoResponse
    {
        public int Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int ItemsCount { get; set; }
        public decimal Total { get; set; }
        public List<CarritoItemResponse> Items { get; set; } = new();
    }

    public class CarritoItemResponse
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? ProductoImagen { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
