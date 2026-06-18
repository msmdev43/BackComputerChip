using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_ofertas")]
    public class ProductosOfertas
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("ofertas_idofertas")]
        public int ofertaId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;

        [ForeignKey(nameof(ofertaId))]
        public virtual Ofertas Ofertas { get; set; } = null!;
    }
}
