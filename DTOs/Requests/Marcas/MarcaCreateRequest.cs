using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerChip.DTOs.Requests.Marcas
{
    public class MarcaCreateRequest
    {
        [Required (ErrorMessage = "El nombre de la marca es obligatorio")]
        [MaxLength(105, ErrorMessage = "El nombre de la marca no puede superar los 105 caracteres")]
        public string nombre { get; set; } = string.Empty;

    }
}
