namespace computerChip.DTOs.Requests.Pedido
{
    public class PedidoCreateRequest
    {
        public int MetodoPagoId { get; set; }
        public int ZonaEnvioId { get; set; }
        public int? OfertaId { get; set; }
    }
}
