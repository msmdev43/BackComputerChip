// DTOs/Responses/ZonaEnvio/ZonaEnvioResponse.cs
namespace computerChip.DTOs.Responses.ZonaEnvio
{
    public class ZonaEnvioResponse
    {
        public int Id { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Costo { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public int PedidosCount { get; set; }
    }

    public class ZonaEnvioCostoResponse
    {
        public string CodigoPostal { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public bool Disponible { get; set; }
    }
}