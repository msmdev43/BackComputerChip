// DTOs/Requests/ZonaEnvio/ZonaEnvioCreateRequest.cs
namespace computerChip.DTOs.Requests.ZonaEnvio
{
    public class ZonaEnvioCreateRequest
    {
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Costo { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }

    public class ZonaEnvioUpdateRequest
    {
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public string? Pais { get; set; }
        public string? Costo { get; set; }
        public string? CodigoPostal { get; set; }
    }

    public class ZonaEnvioCostoRequest
    {
        public string CodigoPostal { get; set; } = string.Empty;
    }
}