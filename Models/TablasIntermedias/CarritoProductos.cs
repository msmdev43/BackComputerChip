using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("carrito_has_productos")]
    public class CarritoProductos
    {
        [Column("carrito_id")]
        public int carritoId { get; set; }

        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("cantidad")]
        public int cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal precioUnitario { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(carritoId))]
        public virtual Carrito Carrito { get; set; } = null!;

        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;
    }
}
