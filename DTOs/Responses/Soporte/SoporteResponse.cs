namespace computerChip.DTOs.Responses.Soporte
{
    public class SoporteResponse
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public bool Atendido { get; set; }
    }
}
