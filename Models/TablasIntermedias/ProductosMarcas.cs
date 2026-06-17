using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_marcas")]
    public class ProductosMarcas
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("marcas_idmarcas")]
        public int marcaId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;

        [ForeignKey(nameof(marcaId))]
        public virtual Marcas Marcas { get; set; } = null!;
    }
}
