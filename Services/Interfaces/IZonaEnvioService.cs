using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IZonaEnvioService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<ZonaEnvio?> GetByIdAsync(int id);
        Task<ZonaEnvio?> GetByCodigoPostalAsync(string codigoPostal);
        Task<ZonaEnvio?> GetByCiudadProvinciaAsync(string ciudad, string provincia);
        Task<IEnumerable<ZonaEnvio>> GetByPaisAsync(string pais);
        Task<IEnumerable<ZonaEnvio>> GetAllActiveAsync();
        Task<decimal> GetCostoEnvioAsync(string codigoPostal);
        Task<bool> ExistsByCodigoPostalAsync(string codigoPostal);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<ZonaEnvio> CreateZonaEnvioAsync(string ciudad, string provincia, string pais, string costo, string codigoPostal);
        Task<bool> UpdateZonaEnvioAsync(int id, string ciudad, string provincia, string pais, string costo, string codigoPostal);
        Task<bool> DeleteZonaEnvioAsync(int id);
        Task<bool> UpdateCostoEnvioAsync(int id, decimal costo);
    }
}