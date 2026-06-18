using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("pedidos_has_item_pedido")]
    public class PedidosItemPedido
    {
        [Column("pedidos_idpedidos")]
        public int pedidosId { get; set; }

        [Column("item_pedido_iditem_pedido")]
        public int itemPedidoId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(pedidosId))]
        public virtual Pedidos Pedidos { get; set; } = null!;

        [ForeignKey(nameof(itemPedidoId))]
        public virtual ItemPedido ItemPedido { get; set; } = null!;
    }
}
