using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("usuarios")]
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("nombreCompleto")]
        [MaxLength(105)]
        public string? nombreCompleto { get; set; }
        [Column("email")]
        [MaxLength(105)]
        public string? email { get; set; }

        [Column("password")]
        public string? password { get; set; }

        [Column("pais")]
        [MaxLength(65)]
        public string? pais { get; set; }
        [Column("provincia")]
        [MaxLength(65)]
        public string? provincia { get; set; }

        [Column("ciudad")]
        [MaxLength(65)]
        public string? ciudad { get; set; }
        [Column("calle")]
        [MaxLength(65)]
        public string? calle { get; set; }

        [Column("numero")]
        [MaxLength(45)]
        public string? numero { get; set; }

        [Column("celular")]
        [MaxLength(25)]
        public string? celular { get; set; }

        [Column("email_verify")]
        public int? emailVerify { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? deletedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<LoginGoogle> LoginGoogle { get; set; } = new List<LoginGoogle>();
        public virtual ICollection<Carrito> Carrito { get; set; } = new List<Carrito>();
        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
        public virtual ICollection<PushToken> PushTokens { get; set; } = new List<PushToken>();
        public virtual ICollection<SantanderToken> SantanderTokens { get; set; } = new List<SantanderToken>();
    }
}
