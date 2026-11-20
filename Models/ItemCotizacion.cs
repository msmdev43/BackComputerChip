
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("cotizaciones_items")]
    public class ItemCotizacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        // ============================================
        // RELACIÓN CON COTIZACIÓN
        // ============================================

        [Column("cotizacion_id")]
        [MaxLength(32)]
        public string CotizacionId { get; set; } = string.Empty;

        [ForeignKey(nameof(CotizacionId))]
        public virtual Cotizacion Cotizacion { get; set; } = null!;

        // ============================================
        // SNAPSHOT DEL PRODUCTO
        // ============================================

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Column("nombre")]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        // ============================================
        // PRECIOS Y CANTIDADES
        // ============================================

        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("subtotal", TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
