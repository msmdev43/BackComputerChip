using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerChip.DTOs.Requests.Usuarios
{
    public class ActualizarUsuarioRequest
    {
        [MaxLength(105, ErrorMessage = "El nombre no puede superar los 105 caracteres")]
        public string? NombreCompleto { get; set; }

        [EmailAddress(ErrorMessage = "El email no es válido")]
        [MaxLength(105, ErrorMessage = "El email no puede superar los 105 caracteres")]
        public string? Email { get; set; }

        [MaxLength(65, ErrorMessage = "El país no puede superar los 65 caracteres")]
        public string? Pais { get; set; }

        [MaxLength(65, ErrorMessage = "La provincia no puede superar los 65 caracteres")]
        public string? Provincia { get; set; }

        [MaxLength(65, ErrorMessage = "La ciudad no puede superar los 65 caracteres")]
        public string? Ciudad { get; set; }

        [MaxLength(65, ErrorMessage = "La calle no puede superar los 65 caracteres")]
        public string? Calle { get; set; }

        [MaxLength(45, ErrorMessage = "El número no puede superar los 45 caracteres")]
        public string? Numero { get; set; }

        [MaxLength(25, ErrorMessage = "El celular no puede superar los 25 caracteres")]
        public string? Celular { get; set; }
    }
}
