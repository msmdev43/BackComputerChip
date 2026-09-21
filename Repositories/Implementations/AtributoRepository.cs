using computerChip.Data;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class AtributoRepository : GenericRepository<Atributos>, IAtributoRepository
    {
        private readonly AppDbContext _context;

        public AtributoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Atributos>> GetAllActiveAsync()
        {
            return await _context.Atributos
                .OrderBy(a => a.nombre)
                .ToListAsync();
        }

        public async Task<Atributos?> GetByNameAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            return await _context.Atributos
                .FirstOrDefaultAsync(a => a.nombre.ToLower() == nombre.ToLower());
        }

        public async Task<Atributos?> GetWithProductosAsync(int id)
        {
            return await _context.Atributos
                .Include(a => a.ProductosAtributos)
                    .ThenInclude(pa => pa.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(a => a.id == id);
        }

        public async Task<bool> ExistsByNameAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            return await _context.Atributos
                .AnyAsync(a => a.nombre.ToLower() == nombre.ToLower());
        }

        public async Task<int> GetProductosCountAsync(int atributoId)
        {
            return await _context.ProductosAtributos
                .CountAsync(pa => pa.atributoId == atributoId);
        }

        // ============================================
        // RELACIÓN PRODUCTO ↔ ATRIBUTO
        // ============================================

        public async Task<bool> AddAtributoToProductoAsync(int productoId, int atributoId, string valor)
        {
            try
            {
                var productoExiste = await _context.Productos
                    .AnyAsync(p => p.id == productoId && p.deletedAt == null);

                var atributoExiste = await _context.Atributos
                    .AnyAsync(a => a.id == atributoId);

                if (!productoExiste || !atributoExiste)
                    return false;

                var yaExiste = await _context.ProductosAtributos
                    .AnyAsync(pa => pa.productoId == productoId && pa.atributoId == atributoId);

                if (yaExiste)
                    return false;

                var nuevo = new ProductosAtributos
                {
                    productoId = productoId,
                    atributoId = atributoId,
                    valor = valor
                };

                await _context.ProductosAtributos.AddAsync(nuevo);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAtributoFromProductoAsync(int productoId, int atributoId)
        {
            try
            {
                var relacion = await _context.ProductosAtributos
                    .FirstOrDefaultAsync(pa => pa.productoId == productoId && pa.atributoId == atributoId);

                if (relacion == null)
                    return false;

                _context.ProductosAtributos.Remove(relacion);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAtributoValorAsync(int productoId, int atributoId, string valor)
        {
            try
            {
                var relacion = await _context.ProductosAtributos
                    .FirstOrDefaultAsync(pa => pa.productoId == productoId && pa.atributoId == atributoId);

                if (relacion == null)
                    return false;

                relacion.valor = valor;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductosAtributos>> GetAtributosByProductoAsync(int productoId)
        {
            return await _context.ProductosAtributos
                .Include(pa => pa.Atributos)
                .Where(pa => pa.productoId == productoId)
                .OrderBy(pa => pa.Atributos.nombre)
                .ToListAsync();
        }

        // ============================================
        // COMPARACIÓN / COMPATIBILIDAD
        // ============================================

        public async Task<IEnumerable<ProductosAtributos>> GetAtributosByProductosAsync(List<int> productoIds)
        {
            return await _context.ProductosAtributos
                .Include(pa => pa.Atributos)
                .Where(pa => productoIds.Contains(pa.productoId))
                .OrderBy(pa => pa.Atributos.nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetProductosConMismoAtributoValorAsync(int atributoId, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return Enumerable.Empty<Productos>();

            var valorLower = valor.ToLower();

            return await _context.Productos
                .Include(p => p.ProductosImagenes)
                    .ThenInclude(pi => pi.Imagenes)
                .Include(p => p.ProductoAtributos)
                    .ThenInclude(pa => pa.Atributos)
                .Where(p => p.deletedAt == null &&
                            p.ProductoAtributos.Any(pa =>
                                pa.atributoId == atributoId &&
                                pa.valor.ToLower() == valorLower))
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductosAtributos>> GetAllAsignacionesAsync()
        {
            return await _context.ProductosAtributos
                .Include(pa => pa.Atributos)
                .Include(pa => pa.Productos)
                .OrderBy(pa => pa.Atributos.nombre)
                .ThenBy(pa => pa.Productos.nombre)
                .ToListAsync();
        }
    }
}