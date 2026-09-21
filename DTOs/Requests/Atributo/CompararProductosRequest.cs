using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Atributo
{
    public class CompararProductosRequest
    {
        [Required]
        [MinLength(2, ErrorMessage = "Se necesitan al menos 2 productos para comparar")]
        public List<int> ProductoIds { get; set; } = new();
    }
}
