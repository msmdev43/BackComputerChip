
using System.Collections.Generic;

namespace computerChip.DTOs.Responses
{
    // ============================================
    // RESPUESTA DE COTIZACIÓN
    // ============================================
    public class CheckoutResponse
    {
        public string CotizacionId { get; set; } = string.Empty;

        public List<ItemCotizacionResponse> Items { get; set; } = new();

        public decimal Subtotal { get; set; }

        public decimal Envio { get; set; }

        public decimal Total { get; set; }

        public bool PuedeConfirmar { get; set; }

        public List<string> Advertencias { get; set; } = new();
    }

    // ============================================
    // PRODUCTOS COTIZADOS
    // ============================================
    public class ItemCotizacionResponse
    {
        public int ProductoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}

