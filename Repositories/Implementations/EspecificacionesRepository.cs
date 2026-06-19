using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class EspecificacionesRepository : GenericRepository<Especificaciones>, IEspecificacionesRepository
    {
        private readonly AppDbContext _context;

        public EspecificacionesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Especificaciones?> GetByTituloAsync(string titulo)
        {
            return await _context.Especificaciones
                .FirstOrDefaultAsync(e => e.titulo.ToLower() == titulo.ToLower());
        }

        public async Task<IEnumerable<Especificaciones>> GetByProductoAsync(int productoId)
        {
            return await _context.Especificaciones
                .Include(e => e.ProductosEspecificaciones)
                .Where(e => e.ProductosEspecificaciones.Any(pe => pe.productoId == productoId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Especificaciones>> GetAllWithProductosAsync()
        {
            return await _context.Especificaciones
                .Include(e => e.ProductosEspecificaciones)
                    .ThenInclude(pe => pe.Productos)
                .OrderBy(e => e.titulo)
                .ToListAsync();
        }

        public async Task<Especificaciones?> GetWithProductosByIdAsync(int id)
        {
            return await _context.Especificaciones
                .Include(e => e.ProductosEspecificaciones)
                    .ThenInclude(pe => pe.Productos)
                .FirstOrDefaultAsync(e => e.id == id);
        }

        public async Task<bool> ExistsByTituloAsync(string titulo)
        {
            return await _context.Especificaciones
                .AnyAsync(e => e.titulo.ToLower() == titulo.ToLower());
        }
    }
}