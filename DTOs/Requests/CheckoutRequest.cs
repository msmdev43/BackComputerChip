
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests
{
    // ============================================
    // CHECKOUT - COTIZACIÓN
    // ============================================
    public class CheckoutRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "El carrito debe contener al menos un producto.")]
        public List<ItemCotizacionRequest> Items { get; set; } = new();

        [Required]
        public CompradorRequest Comprador { get; set; } = new();

        [Required]
        public EntregaRequest Entrega { get; set; } = new();

        [StringLength(1000)]
        public string? Observaciones { get; set; }
    }

    // ============================================
    // PRODUCTOS DE LA COTIZACIÓN
    // ============================================
    public class ItemCotizacionRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El producto no es válido.")]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "La cantidad debe estar entre 1 y 99.")]
        public int Cantidad { get; set; }
    }

    // ============================================
    // DATOS DEL COMPRADOR
    // ============================================
    public class CompradorRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(40)]
        public string Telefono { get; set; } = string.Empty;
    }

    // ============================================
    // DATOS DE ENTREGA
    // ============================================
    public class EntregaRequest
    {
        [Required]
        [RegularExpression(
            "^(envio|retiro)$",
            ErrorMessage = "El tipo de entrega debe ser envio o retiro."
        )]
        public string Tipo { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Provincia { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(20)]
        public string? CodigoPostal { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }
    }
}

