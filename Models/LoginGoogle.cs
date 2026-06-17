using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("login_google")]
    public class LoginGoogle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("usuario_id")]
        public int usuarioId { get; set; }

        [Column("google_sub")]
        [MaxLength(255)]
        [Required]
        public string googleSub { get; set; } = string.Empty;

        [Column("email")]
        [MaxLength(100)]
        [Required]
        public string email { get; set; } = string.Empty;

        [Column("email_verificado")]
        public int? emailVerificado { get; set; }

        [Column("nombre")]
        [MaxLength(150)]
        public string? nombre { get; set; }
        [Column("avatar_url")]
        public string? avatarUrl { get; set; }

        [Column("refresh_token")]
        public string? refreshToken { get; set; }

        [Column("ultimo_login")]
        public DateTime? ultimoLogin { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? deletedAt { get; set; }

        // Relaciones
        [ForeignKey(nameof(usuarioId))]
        [InverseProperty("LoginGoogle")]
        public virtual Usuarios Usuarios { get; set; } = null!;
    }
}
