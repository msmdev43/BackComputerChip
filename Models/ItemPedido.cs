using computerChip.Models.TablasIntermedias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("item_pedido")]
    public class ItemPedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("cantProducto")]
        public int cantidad { get; set; }

        [Column("subtotal")]
        public decimal subtotal { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<PedidosItemPedido> PedidosItemPedido { get; set; } = new List<PedidosItemPedido>();

        public virtual ICollection<ItemPedidoProductos> ItemPedidoProductos { get; set; } = new List<ItemPedidoProductos>();
    }
}
