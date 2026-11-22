
using computerChip.Models.Enum;
using computerChip.Models.TablasIntermedias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("pedidos")]
    public class Pedidos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("cotizacion_id")]
        [MaxLength(32)]
        public string? CotizacionId { get; set; } = string.Empty;

        [Column("idempotency_key")]
        [MaxLength(100)]
        public string? IdempotencyKey { get; set; } = string.Empty;

        [Column("request_hash")]
        [MaxLength(64)]
        public string? RequestHash { get; set; } = string.Empty;

        [Column("metodos_pago_idmetodos_pago")]
        public int? MetodoPagoId { get; set; }

        [Column("zona_envio_idenvio")]
        public int ZonaEnvioId { get; set; }

        [Column("ofertas_idofertas")]
        public int? OfertaId { get; set; }

        [Column("estado")]
        public EstadoPedido estado { get; set; } =
            EstadoPedido.PENDIENTE;

        [Column("estado_pago")]
        [MaxLength(30)]
        public string estadoPago { get; set; } = "NO_SOLICITADO";

        // Datos históricos del comprador
        [Column("comprador_nombre")]
        [MaxLength(150)]
        public string compradorNombre { get; set; } = string.Empty;

        [Column("comprador_email")]
        [MaxLength(254)]
        public string compradorEmail { get; set; } = string.Empty;

        [Column("comprador_telefono")]
        [MaxLength(40)]
        public string compradorTelefono { get; set; } = string.Empty;

        // Datos históricos de entrega
        [Column("entrega_tipo")]
        [MaxLength(20)]
        public string entregaTipo { get; set; } = "envio";

        [Column("entrega_provincia")]
        [MaxLength(100)]
        public string entregaProvincia { get; set; } = string.Empty;

        [Column("entrega_ciudad")]
        [MaxLength(100)]
        public string entregaCiudad { get; set; } = string.Empty;

        [Column("entrega_codigo_postal")]
        [MaxLength(20)]
        public string entregaCodigoPostal { get; set; } = string.Empty;

        [Column("entrega_direccion")]
        [MaxLength(200)]
        public string entregaDireccion { get; set; } = string.Empty;

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? observaciones { get; set; }

        // Importes históricos
        [Column("subtotal", TypeName = "decimal(10,2)")]
        public decimal subtotal { get; set; }

        [Column("envio", TypeName = "decimal(10,2)")]
        public decimal envio { get; set; }

        [Column("total", TypeName = "decimal(10,2)")]
        public decimal total { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // Relaciones
        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuarios Usuarios { get; set; } = null!;

        [ForeignKey(nameof(CotizacionId))]
        public virtual Cotizacion? Cotizacion { get; set; }

        [ForeignKey(nameof(MetodoPagoId))]
        public virtual MetodoPago? MetodoPago { get; set; }

        [ForeignKey(nameof(ZonaEnvioId))]
        public virtual ZonaEnvio ZonaEnvio { get; set; } = null!;

        [ForeignKey(nameof(OfertaId))]
        public virtual Ofertas? Ofertas { get; set; }

        public virtual ICollection<ItemPedido> Items { get; set; }
            = new List<ItemPedido>();

        public virtual ICollection<PedidosItemPedido> PedidosItemPedido { get; set; }
            = new List<PedidosItemPedido>();
    }
}