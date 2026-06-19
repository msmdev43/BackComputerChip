using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class CategoriaRepository : GenericRepository<Categorias>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorias>> GetAllActiveAsync()
        {
            return await _context.Categorias
                .Where(c => c.deletedAt == null)
                .OrderBy(c => c.nombre)
                .ToListAsync();
        }

        public async Task<Categorias?> GetByNameAsync(string nombre)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.nombre.ToLower() == nombre.ToLower() && c.deletedAt == null);
        }

        public async Task<Categorias?> GetWithProductosAsync(int id)
        {
            return await _context.Categorias
                .Include(c => c.CategoriasProductos)
                    .ThenInclude(cp => cp.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(c => c.id == id && c.deletedAt == null);
        }

        public async Task<IEnumerable<Categorias>> GetAllWithProductosAsync()
        {
            return await _context.Categorias
                .Include(c => c.CategoriasProductos)
                    .ThenInclude(cp => cp.Productos)
                .Where(c => c.deletedAt == null)
                .OrderBy(c => c.nombre)
                .ToListAsync();
        }

        public async Task<int> GetProductCountByCategoriaAsync(int categoriaId)
        {
            return await _context.CategoriasProductos
                .CountAsync(cp => cp.categoriaId == categoriaId);
        }

        public async Task<bool> ExistsByNameAsync(string nombre)
        {
            return await _context.Categorias
                .AnyAsync(c => c.nombre.ToLower() == nombre.ToLower() && c.deletedAt == null);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null || categoria.deletedAt != null)
                    return false;

                categoria.deletedAt = DateTime.Now;
                categoria.updatedAt = DateTime.Now;
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
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null || categoria.deletedAt == null)
                    return false;

                categoria.deletedAt = null;
                categoria.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}