using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Productos
{
    public class ProductoCreateRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string Garantia { get; set; } = string.Empty;
        public bool Stock { get; set; }  // ✅ true = disponible, false = no disponible
        public int EnvioGratis { get; set; } // 0 = no, 1 = sí
        public string? CodigoSerie { get; set; }
        public List<int> CategoriaIds { get; set; } = new();
        public List<int> MarcaIds { get; set; } = new();
        public List<int> EspecificacionIds { get; set; } = new();
        public List<ProductoAtributoRequest> Atributos { get; set; } = new();
        public List<IFormFile>? Imagenes { get; set; }
    }

    // Sub‑DTOs para especificaciones y atributos (clave‑valor)
    public class EspecificacionRequest
    {
        [Required]
        public int EspecificacionId { get; set; }  // ID de la especificación (tabla Especificaciones)
 
    }

    public class ProductoAtributoRequest
    {
        public int AtributoId { get; set; }
        public string Valor { get; set; } = string.Empty;
    }
}