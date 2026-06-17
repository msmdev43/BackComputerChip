using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("metodos_pago")]
    public class MetodoPago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("tipo")]
        [MaxLength(45)]
        [Required]
        public string tipo { get; set; } = string.Empty;

        [Column("descuento")]
        public decimal descuento { get; set; }

        [Column("tieneDesc")]
        public int tieneDesc { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
