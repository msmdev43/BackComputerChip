using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_imagenes")]
    public class ProductosImagenes
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("imagenes_idimagenes")]
        public int imagenId { get; set; }

        [Column("orden")]
        public int orden { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;

        [ForeignKey(nameof(imagenId))]
        public virtual Imagenes Imagenes { get; set; } = null!;
    }
}
