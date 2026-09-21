using System.ComponentModel.DataAnnotations;

namespace computerChip.DTOs.Requests.Atributo
{
    public class AsignarAtributoRequest
    {
        [Required]
        public int AtributoId { get; set; }

        [Required]
        [StringLength(65)]
        public string Valor { get; set; } = string.Empty;
    }

    public class AsignarAtributosRequest
    {
        [Required]
        public List<AsignarAtributoRequest> Atributos { get; set; } = new();
    }
}
