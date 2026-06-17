using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("santander_token")]
    public class SantanderToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("usuario_id")]
        public int usuarioId { get; set; }

        [Column("access_token")]
        public string accessToken { get; set; } = string.Empty;

        [Column("refresh_token")]
        public string refreshToken { get; set; } = string.Empty;

        [Column("expires_in")]
        public int expiresIn { get; set; }

        [Column("token_type")]
        [MaxLength(50)]
        public string? tokenType { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(usuarioId))]
        public virtual Usuarios Usuarios { get; set; } = null!;
    }
}
