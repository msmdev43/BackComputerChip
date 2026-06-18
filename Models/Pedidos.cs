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
    [Table("pedidos")]
    public class Pedidos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("metodos_pago_idmetodos_pago")]
        public int MetodoPagoId { get; set; }

        [Column("zona_envio_idenvio")]
        public int ZonaEnvioId { get; set; }

        [Column("ofertas_idofertas")]
        public int? OfertaId { get; set; }

        [Column("estado")]
        public string estado { get; set; } = "pendiente";

        [Column("total")]
        public decimal total { get; set; }

        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuarios Usuarios { get; set; } = null!;

        [ForeignKey(nameof(MetodoPagoId))]
        public virtual MetodoPago MetodoPago { get; set; } = null!;

        [ForeignKey(nameof(ZonaEnvioId))]
        public virtual ZonaEnvio ZonaEnvio { get; set; } = null!;

        [ForeignKey(nameof(OfertaId))]
        public virtual Ofertas? Ofertas { get; set; }

        public virtual ICollection<ItemPedido> Items { get; set; } = new List<ItemPedido>();

        public virtual ICollection<PedidosItemPedido> PedidosItemPedido { get; set; } = new List<PedidosItemPedido>();
    }
}
