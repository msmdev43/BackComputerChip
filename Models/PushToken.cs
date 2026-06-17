using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("push_token")]
    public class PushToken
    {
        [Key]
        public int id { get; set; }

        [Column("Admin_idAdmin")]
        public int? adminId { get; set; }

        [Column("usuarios_idusuarios")]
        public int? usuarioId { get; set; }

        [Column("token")]
        public string token { get; set; } = string.Empty;

        [Column("dispositivo")]
        [MaxLength(50)]
        public string? dispositivo { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(adminId))]
        public virtual Admin? Admin { get; set; }

        [ForeignKey(nameof(usuarioId))]
        public virtual Usuarios? Usuarios { get; set; }
    }
}
