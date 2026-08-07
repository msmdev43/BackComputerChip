// DTOs/Responses/Pedido/PedidoResponse.cs
namespace computerChip.DTOs.Responses.Pedido
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoColor { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string ZonaEnvio { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public List<PedidoItemResponse> Items { get; set; } = new();
        public UsuarioResumenResponse Usuario { get; set; } = new();
        public OfertaResumenResponse? Oferta { get; set; }
    }

    public class PedidoItemResponse
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? ProductoImagen { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class UsuarioResumenResponse
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Celular { get; set; }
    }

    public class OfertaResumenResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
    }

    public class PedidoListResponse
    {
        public int Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public int ItemsCount { get; set; }
    }

    public class PedidoStatsResponse
    {
        public int TotalPedidos { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosConfirmados { get; set; }
        public int PedidosEnviados { get; set; }
        public int PedidosEntregados { get; set; }
        public int PedidosCancelados { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal PromedioVenta { get; set; }
        public decimal MaxVenta { get; set; }
        public int PedidosHoy { get; set; }
    }
}