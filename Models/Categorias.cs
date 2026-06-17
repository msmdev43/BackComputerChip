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
    [Table("categorias")]
    public class Categorias
    {
        [Key]
        public int id { get; set; }

        [Column("nombre")]
        [MaxLength(65)]
        [Required]
        public string nombre { get; set; } = string.Empty;

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? deletedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<CategoriasProductos> CategoriasProductos { get; set; } = new List<CategoriasProductos>();
    }
}
