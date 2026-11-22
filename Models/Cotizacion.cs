
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("cotizaciones")]
    public class Cotizacion
    {
        [Key]
        [MaxLength(32)]
        [Column("id")]
        public string id { get; set; } = Guid.NewGuid().ToString("N");

        // ============================================
        // USUARIO PROPIETARIO
        // ============================================

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuarios Usuario { get; set; } = null!;

        // ============================================
        // DATOS DEL COMPRADOR
        // ============================================

        [Column("comprador_nombre")]
        [MaxLength(150)]
        public string compradorNombre { get; set; } = string.Empty;

        [Column("comprador_email")]
        [MaxLength(254)]
        public string compradorEmail { get; set; } = string.Empty;

        [Column("comprador_telefono")]
        [MaxLength(40)]
        public string compradorTelefono { get; set; } = string.Empty;

        // ============================================
        // DATOS DE ENTREGA
        // ============================================

        [Column("entrega_tipo")]
        [MaxLength(20)]
        public string entregaTipo { get; set; } = string.Empty;

        [Column("entrega_provincia")]
        [MaxLength(100)]
        public string? entregaProvincia { get; set; }

        [Column("entrega_ciudad")]
        [MaxLength(100)]
        public string? entregaCiudad { get; set; }

        [Column("entrega_codigo_postal")]
        [MaxLength(20)]
        public string? entregaCodigoPostal { get; set; }

        [Column("entrega_direccion")]
        [MaxLength(200)]
        public string? entregaDireccion { get; set; }

        // ============================================
        // INFORMACIÓN ADICIONAL
        // ============================================

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? observaciones { get; set; }

        // ============================================
        // IMPORTES
        // ============================================

        [Column("subtotal", TypeName = "decimal(10,2)")]
        public decimal subtotal { get; set; }

        [Column("envio", TypeName = "decimal(10,2)")]
        public decimal envio { get; set; }

        [Column("total", TypeName = "decimal(10,2)")]
        public decimal total { get; set; }

        // ============================================
        // ESTADO DE LA COTIZACIÓN
        // ============================================

        [Column("puede_confirmar")]
        public bool puedeConfirmar { get; set; }

        [Column("advertencias", TypeName = "text")]
        public string? advertencias { get; set; }

        // ============================================
        // VENCIMIENTO Y FECHAS
        // ============================================

        [Column("fecha_creacion")]
        public DateTime fechaCreacion { get; set; }

        [Column("fecha_vencimiento")]
        public DateTime fechaVencimiento { get; set; }

        [Column("utilizada")]
        public bool utilizada { get; set; }

        [Column("zona_envio_id")]
        public int? ZonaEnvioId { get; set; }

        [ForeignKey(nameof(ZonaEnvioId))]
        public virtual ZonaEnvio? ZonaEnvio { get; set; }

        public virtual ICollection<ItemCotizacion> Items { get; set; }
            = new List<ItemCotizacion>();
    }
}
