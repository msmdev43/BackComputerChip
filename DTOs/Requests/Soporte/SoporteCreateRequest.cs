namespace computerChip.DTOs.Requests.Soporte
{
    public class SoporteCreateRequest
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
