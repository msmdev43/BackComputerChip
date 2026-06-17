using computerChip.Models.TablasIntermedias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("carrito")]
    public class Carrito
    {
        [Key]
        public int id { get; set; }

        [Column("usuario_id")]
        public int usuarioId { get; set; }

        [Column("estado")]
        public string estado { get; set; } = "activo";

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(usuarioId))]
        public virtual Usuarios Usuarios { get; set; } = null!;

        public virtual ICollection<CarritoProductos> CarritoProductos { get; set; } = new List<CarritoProductos>();
    }
}
