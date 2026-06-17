using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("zona_envio")]
    public class ZonaEnvio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("ciudad")]
        [MaxLength(65)]
        [Required]
        public string ciudad { get; set; } = string.Empty;

        [Column("provincia")]
        [MaxLength(65)]
        [Required]
        public string provincia { get; set; } = string.Empty;

        [Column("pais")]
        [MaxLength(65)]
        [Required]
        public string pais { get; set; } = string.Empty;

        [Column("costo")]
        [MaxLength(65)]
        [Required]
        public string costo { get; set; } = string.Empty;

        [Column("codigopostal")]
        [MaxLength(45)]
        [Required]
        public string codigoPostal { get; set; } = string.Empty;

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
