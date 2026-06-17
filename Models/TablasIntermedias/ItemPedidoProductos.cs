using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("item_pedido_has_productos")]
    public class ItemPedidoProductos
    {
        [Column("item_pedido_iditem_pedido")]
        public int itemPedidoId { get; set; }

        [Column("productos_idproductos")]
        public int productoId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(itemPedidoId))]
        public virtual ItemPedido itemPedido { get; set; } = null!;

        [ForeignKey(nameof(productoId))]
        public virtual Productos producto { get; set; } = null!;
    }
}
