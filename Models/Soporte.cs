using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("soporte")]
    public class Soporte
    {
        [Key]
        public int id { get; set; }

        [Column("nombreCompleto")]
        [MaxLength(65)]
        [Required]
        public string nombreCompleto { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime fecha { get; set; }

        [Column("email")]
        [MaxLength(65)]
        public string? email { get; set; }

        [Column("telefono")]
        [MaxLength(65)]
        public string? telefono { get; set; }

        [Column("mensaje")]
        public string mensaje { get; set; } = string.Empty;
    }
}
