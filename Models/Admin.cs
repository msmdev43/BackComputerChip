using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        public int id { get; set; }

        [Column("usuario")]
        [MaxLength(65)]
        [Required]
        public string usuario { get; set; } = string.Empty;

        [Column("password")]
        public string password { get; set; } = string.Empty;

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<PushToken> PushTokens { get; set; } = new List<PushToken>();
    }
}
