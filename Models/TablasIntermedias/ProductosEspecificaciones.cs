using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_especificaciones")]
    public class ProductosEspecificaciones
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("especificaciones_idespecificaciones")]
        public int especificacionId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos producto { get; set; } = null!;

        [ForeignKey(nameof(especificacionId))]
        public virtual Especificaciones especificacion { get; set; } = null!;
    }
}
