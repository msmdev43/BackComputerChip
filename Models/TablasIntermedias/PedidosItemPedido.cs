using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("pedidos_has_item_pedido")]
    public class PedidosItemPedido
    {
        [Column("pedidos_idpedidos")]
        public int pedidoId { get; set; }

        [Column("item_pedido_iditem_pedido")]
        public int itemPedidoId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(pedidoId))]
        public virtual Pedidos pedido { get; set; } = null!;

        [ForeignKey(nameof(itemPedidoId))]
        public virtual ItemPedido itemPedido { get; set; } = null!;
    }
}
