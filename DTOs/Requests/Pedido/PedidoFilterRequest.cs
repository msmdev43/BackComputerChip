namespace computerChip.DTOs.Requests.Pedido
{
    public class PedidoFilterRequest
    {
        public string? Estado { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal? TotalMin { get; set; }
        public decimal? TotalMax { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
