using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Productos
{
    public class CrearProductoRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Precio { get; set; }

        public decimal? PrecioOferta { get; set; }

        [Required(ErrorMessage = "La garantía es obligatoria")]
        [MaxLength(85, ErrorMessage = "La garantía no puede superar los 85 caracteres")]
        public string Garantia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El stock (disponibilidad) es obligatorio")]
        public bool Stock { get; set; }  // true = disponible, false = agotado

        public int EnvioGratis { get; set; }  // Podría ser 0 o 1, o un indicador

        [MaxLength(85, ErrorMessage = "El código de serie no puede superar los 85 caracteres")]
        public string? CodigoSerie { get; set; }

        // Relaciones: se envían listas de IDs para asociar
        public List<int> CategoriaIds { get; set; } = new();
        public List<int> MarcaIds { get; set; } = new();   // Si un producto puede tener varias marcas (raro) o una sola. Ajusta según tu lógica.
        public List<string> ImagenesUrls { get; set; } = new(); // Si solo guardas URLs
        public List<EspecificacionRequest> Especificaciones { get; set; } = new();
        public List<AtributoRequest> Atributos { get; set; } = new();
    }

    // Sub‑DTOs para especificaciones y atributos (clave‑valor)
    public class EspecificacionRequest
    {
        [Required]
        public int EspecificacionId { get; set; }  // ID de la especificación (tabla Especificaciones)
 
    }

    public class AtributoRequest
    {
        [Required]
        public int AtributoId { get; set; }  // ID del atributo (tabla Atributos)
        
    }
}