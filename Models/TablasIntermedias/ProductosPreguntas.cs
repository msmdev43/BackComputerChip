using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_preguntas")]
    public class ProductosPreguntas
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("preguntas_idpreguntas")]
        public int preguntaId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;

        [ForeignKey(nameof(preguntaId))]
        public virtual Preguntas Preguntas { get; set; } = null!;
    }
}
