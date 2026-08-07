namespace computerChip.DTOs.Responses.MetodoPago
{
    public class MetodoPagoResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public bool TieneDesc { get; set; }
        public string DescuentoText => TieneDesc ? $"{Descuento}% de descuento" : "Sin descuento";
        public int PedidosCount { get; set; }
    }
}
