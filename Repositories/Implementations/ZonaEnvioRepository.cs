using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class ZonaEnvioRepository : GenericRepository<ZonaEnvio>, IZonaEnvioRepository
    {
        private readonly AppDbContext _context;

        public ZonaEnvioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ZonaEnvio?> GetByCodigoPostalAsync(string codigoPostal)
        {
            return await _context.ZonasEnvio
                .FirstOrDefaultAsync(z => z.codigoPostal == codigoPostal);
        }

        public async Task<ZonaEnvio?> GetByCiudadProvinciaAsync(string ciudad, string provincia)
        {
            return await _context.ZonasEnvio
                .FirstOrDefaultAsync(z => z.ciudad == ciudad && z.provincia == provincia);
        }

        public async Task<IEnumerable<ZonaEnvio>> GetByPaisAsync(string pais)
        {
            return await _context.ZonasEnvio
                .Where(z => z.pais == pais)
                .OrderBy(z => z.ciudad)
                .ToListAsync();
        }

        public async Task<IEnumerable<ZonaEnvio>> GetAllActiveAsync()
        {
            return await _context.ZonasEnvio
                .OrderBy(z => z.pais)
                .ThenBy(z => z.provincia)
                .ThenBy(z => z.ciudad)
                .ToListAsync();
        }

        public async Task<decimal> GetCostoEnvioAsync(string codigoPostal)
        {
            var zona = await GetByCodigoPostalAsync(codigoPostal);
            if (zona == null)
                return 0;

            return decimal.TryParse(zona.costo, out var costo) ? costo : 0;
        }

        public async Task<bool> ExistsByCodigoPostalAsync(string codigoPostal)
        {
            return await _context.ZonasEnvio
                .AnyAsync(z => z.codigoPostal == codigoPostal);
        }
    }
}