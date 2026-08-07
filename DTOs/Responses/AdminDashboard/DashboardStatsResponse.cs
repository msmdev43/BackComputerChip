// DTOs/Responses/AdminDashboard/DashboardStatsResponse.cs
using computerChip.DTOs.Responses.Productos;

namespace computerChip.DTOs.Responses.AdminDashboard
{
    public class DashboardStatsResponse
    {
        // Pedidos
        public int TotalPedidos { get; set; }
        public int PedidosHoy { get; set; }
        public int PedidosPendientes { get; set; }
        public int ProductosPedidos { get; set; }

        // Usuarios
        public int TotalUsuarios { get; set; }
        public int NuevosUsuariosMes { get; set; }
        public int UsuariosGoogle { get; set; }

        // Productos
        public int TotalProductos { get; set; }
        public int ProductosSinStock { get; set; }
        public int TotalCategorias { get; set; }
        public int ProductosConCategoria { get; set; }

        // Ventas
        public decimal VentasTotales { get; set; }
        public decimal VentasMes { get; set; }
        public decimal VentasSemana { get; set; }
        public decimal PromedioVenta { get; set; }
        public decimal MaxVenta { get; set; }

        // Ofertas
        public int OfertasActivas { get; set; }
        public decimal DescuentoMaximo { get; set; }
    }

    public class DashboardVentasResponse
    {
        public List<DashboardVentaDiaria> VentasDiarias { get; set; } = new();
        public List<DashboardVentaMensual> VentasMensuales { get; set; } = new();
        public decimal Total { get; set; }
        public decimal Promedio { get; set; }
    }

    public class DashboardVentaDiaria
    {
        public DateTime Fecha { get; set; }
        public int Pedidos { get; set; }
        public decimal Total { get; set; }
    }

    public class DashboardVentaMensual
    {
        public int Mes { get; set; }
        public int Año { get; set; }
        public int Pedidos { get; set; }
        public decimal Total { get; set; }
    }

    public class DashboardProductosResponse
    {
        public List<ProductoMasVendidoResponse> MasVendidos { get; set; } = new();
        public List<ProductoResponse> SinStock { get; set; } = new();
        public List<ProductoResponse> Nuevos { get; set; } = new();
    }

    public class ProductoMasVendidoResponse : ProductoListResponse
    {
        public int TotalVendido { get; set; }
        public decimal IngresoGenerado { get; set; }
    }
}