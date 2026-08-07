// DTOs/Responses/Usuario/UsuarioResponse.cs
using computerChip.DTOs.Responses.Carrito;
using computerChip.DTOs.Responses.Pedido;
using computerChip.DTOs.Responses.PushToken;

namespace computerChip.DTOs.Responses.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Pais { get; set; }
        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Celular { get; set; }
        public bool EmailVerify { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGoogleUser { get; set; }
        public int PedidosCount { get; set; }
        public int CarritoItemsCount { get; set; }
    }

    public class UsuarioDetailResponse
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Pais { get; set; }
        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Celular { get; set; }
        public bool EmailVerify { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGoogleUser { get; set; }
        public List<PedidoListResponse> Pedidos { get; set; } = new();
        public CarritoResponse? Carrito { get; set; }
        public List<PushTokenResponse> PushTokens { get; set; } = new();
    }

    public class UsuarioAdminResponse
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Celular { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGoogleUser { get; set; }
        public int PedidosCount { get; set; }
        public decimal TotalGastado { get; set; }
        public DateTime? UltimoPedido { get; set; }
        public bool IsActive { get; set; }
    }
}