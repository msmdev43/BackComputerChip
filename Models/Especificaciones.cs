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
    [Table("especificaciones")]
    public class Especificaciones
    {
        [Key]
        public int id { get; set; }

        [Column("titulo")]
        [MaxLength(65)]
        [Required]
        public string titulo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string descripcion { get; set; } = string.Empty;

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<ProductosEspecificaciones> ProductosEspecificaciones { get; set; } = new List<ProductosEspecificaciones>();
    }
}
