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
    [Table("preguntas")]
    public class Preguntas
    {
        [Key]
        public int id { get; set; }

        [Column("textopregunta")]
        public string textoPregunta { get; set; } = string.Empty;

        [Column("textorespuesta")]
        public string textoRespuesta { get; set; } = string.Empty;

        // ============================================
        // RELACIONES
        // ============================================
        public virtual ICollection<ProductosPreguntas> ProductosPreguntas { get; set; } = new List<ProductosPreguntas>();
    }
}
