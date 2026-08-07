namespace computerChip.DTOs.Responses.AdminDashboard
{
    public class DashboardStatsResponse
    {
        public int TotalPedidos { get; set; }
        public int PedidosHoy { get; set; }
        public int CantidadProductosPedidos { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalCategorias { get; set; }
        public int ProductosConCategoria { get; set; }
        public decimal VentasTotales { get; set; }
        public decimal VentasDelMes { get; set; }
    }
}
