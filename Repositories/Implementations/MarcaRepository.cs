using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class MarcaRepository : GenericRepository<Marcas>, IMarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marcas>> GetAllActiveAsync()
        {
            return await _context.Marcas
                .Where(m => m.deletedAt == null)
                .OrderBy(m => m.nombre)
                .ToListAsync();
        }

        public async Task<Marcas?> GetByNameAsync(string nombre)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(m => m.nombre.ToLower() == nombre.ToLower() && m.deletedAt == null);
        }

        public async Task<Marcas?> GetWithProductosAsync(int id)
        {
            return await _context.Marcas
                .Include(m => m.ProductosMarcas)
                    .ThenInclude(mp => mp.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(m => m.id == id && m.deletedAt == null);
        }

        public async Task<IEnumerable<Marcas>> GetAllWithProductosAsync()
        {
            return await _context.Marcas
                .Include(m => m.ProductosMarcas)
                    .ThenInclude(mp => mp.Productos)
                .Where(m => m.deletedAt == null)
                .OrderBy(m => m.nombre)
                .ToListAsync();
        }

        public async Task<int> GetProductCountByMarcaAsync(int marcaId)
        {
            return await _context.ProductosMarcas
                .CountAsync(mp => mp.marcaId == marcaId);
        }

        public async Task<bool> ExistsByNameAsync(string nombre)
        {
            return await _context.Marcas
                .AnyAsync(m => m.nombre.ToLower() == nombre.ToLower() && m.deletedAt == null);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var marca = await _context.Marcas.FindAsync(id);
                if (marca == null || marca.deletedAt != null)
                    return false;

                marca.deletedAt = DateTime.Now;
                marca.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            try
            {
                var marca = await _context.Marcas.FindAsync(id);
                if (marca == null || marca.deletedAt == null)
                    return false;

                marca.deletedAt = null;
                marca.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}