using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IZonaEnvioRepository : IRepository<ZonaEnvio>
    {
        Task<ZonaEnvio?> GetByCodigoPostalAsync(string codigoPostal);
        Task<ZonaEnvio?> GetByCiudadProvinciaAsync(string ciudad, string provincia);
        Task<IEnumerable<ZonaEnvio>> GetByPaisAsync(string pais);
        Task<IEnumerable<ZonaEnvio>> GetAllActiveAsync();
        Task<decimal> GetCostoEnvioAsync(string codigoPostal);
        Task<bool> ExistsByCodigoPostalAsync(string codigoPostal);
    }
}