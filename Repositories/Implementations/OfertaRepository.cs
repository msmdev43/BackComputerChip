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
    public class OfertaRepository : GenericRepository<Ofertas>, IOfertaRepository
    {
        private readonly AppDbContext _context;

        public OfertaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ofertas>> GetAllActiveAsync()
        {
            return await _context.Ofertas
                .Where(o => o.deletedAt == null)
                .OrderBy(o => o.titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ofertas>> GetByTipoAsync(string tipoOferta)
        {
            return await _context.Ofertas
                .Where(o => o.tipoOferta == tipoOferta && o.deletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ofertas>> GetAllWithProductosAsync()
        {
            return await _context.Ofertas
                .Include(o => o.ProductosOfertas)
                    .ThenInclude(po => po.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .Where(o => o.deletedAt == null)
                .OrderBy(o => o.titulo)
                .ToListAsync();
        }

        public async Task<Ofertas?> GetWithProductosAsync(int id)
        {
            return await _context.Ofertas
                .Include(o => o.ProductosOfertas)
                    .ThenInclude(po => po.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(o => o.id == id && o.deletedAt == null);
        }

        public async Task<decimal> GetMaxDescuentoAsync()
        {
            return await _context.Ofertas
                .Where(o => o.deletedAt == null)
                .MaxAsync(o => o.descuento);
        }

        public async Task<IEnumerable<Ofertas>> GetOfertasVigentesAsync()
        {
            return await _context.Ofertas
                .Include(o => o.ProductosOfertas)
                    .ThenInclude(po => po.Productos)
                .Where(o => o.deletedAt == null)
                .OrderByDescending(o => o.createdAt)
                .ToListAsync();
        }

        public async Task<bool> HasActiveOfertaForProductoAsync(int productoId)
        {
            return await _context.Ofertas
                .AnyAsync(o => o.ProductosOfertas.Any(po => po.productoId == productoId)
                               && o.deletedAt == null);
        }

        public async Task<IEnumerable<Ofertas>> GetByDescuentoMayorAsync(decimal descuentoMinimo)
        {
            return await _context.Ofertas
                .Include(o => o.ProductosOfertas)
                    .ThenInclude(po => po.Productos)
                .Where(o => o.descuento >= descuentoMinimo && o.deletedAt == null)
                .OrderByDescending(o => o.descuento)
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var oferta = await _context.Ofertas.FindAsync(id);
                if (oferta == null || oferta.deletedAt != null)
                    return false;

                oferta.deletedAt = DateTime.Now;
                oferta.updatedAt = DateTime.Now;
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
                var oferta = await _context.Ofertas.FindAsync(id);
                if (oferta == null || oferta.deletedAt == null)
                    return false;

                oferta.deletedAt = null;
                oferta.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}