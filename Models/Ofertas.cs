using computerChip.Models.TablasIntermedias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerChip.Models
{
    [Table("ofertas")]
    public class Ofertas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("titulo")]
        [MaxLength(255)]
        [Required]
        public string titulo { get; set; } = string.Empty;

        [Column("subtitulo")]
        [MaxLength(85)]
        [Required]
        public string subtitulo { get; set; } = string.Empty;

        [Column("tipoOferta")]
        [MaxLength(85)]
        [Required]
        public string tipoOferta { get; set; } = string.Empty;

        [Column("tipoDescuento")]
        [MaxLength(85)]
        [Required]
        public string tipoDescuento { get; set; } = string.Empty;

        [Column("descuento")]
        public decimal descuento { get; set; }

        [Column("precioOriginal")]
        public decimal precioOriginal { get; set; }

        [Column("precioOferta")]
        public decimal precioOferta { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? deletedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<ProductosOfertas> ProductosOfertas { get; set; } = new List<ProductosOfertas>();
        public virtual ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
