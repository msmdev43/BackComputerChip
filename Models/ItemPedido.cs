
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

        [Column("subtotal", TypeName = "decimal(10,2)")]
        public decimal subtotal { get; set; }

        // Snapshot histórico del producto
        [Column("nombre_producto")]
        [MaxLength(255)]
        public string nombreProducto { get; set; } = string.Empty;

        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal precioUnitario { get; set; }

        // Relaciones
        public virtual ICollection<PedidosItemPedido> PedidosItemPedido { get; set; }
            = new List<PedidosItemPedido>();

        public virtual ICollection<ItemPedidoProductos> ItemPedidoProductos { get; set; }
            = new List<ItemPedidoProductos>();
    }
}
