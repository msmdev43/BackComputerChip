using computerChip.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Pedido
{
    public class PedidoUpdateEstadoRequest
    {
        public EstadoPedido Estado { get; set; }
    }
}
