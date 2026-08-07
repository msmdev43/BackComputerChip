namespace computerChip.DTOs.Requests.Pedido
{
    public class PedidoUpdateEstadoRequest
    {
        public string Estado { get; set; } = string.Empty; // "CONFIRMADO", "ENVIADO", "ENTREGADO", "CANCELADO"
    }
}
