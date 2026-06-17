using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("productos_has_atributos")]
    public class ProductosAtributos
    {
        [Column("productos_idproductos")]
        public int productoId { get; set; }

        [Column("atributos_idatributos")]
        public int atributoId { get; set; }

        [Column("valor")]
        [MaxLength(65)]
        [Required]
        public string valor { get; set; } = string.Empty;

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;

        [ForeignKey(nameof(atributoId))]
        public virtual Atributos Atributos { get; set; } = null!;
    }
}
