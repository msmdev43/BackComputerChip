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
        public string Nombre { get; set; } = string.Empty;

    }
}
