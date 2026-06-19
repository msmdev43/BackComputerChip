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
    public class ProductoRepository : GenericRepository<Productos>, IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Productos>> GetByCategoriaAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.CategoriasProductos)
                .Where(p => p.CategoriasProductos.Any(cp => cp.categoriaId == categoriaId)
                            && p.deletedAt == null)
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetByMarcaAsync(int marcaId)
        {
            return await _context.Productos
                .Include(p => p.ProductosMarcas)
                .Where(p => p.ProductosMarcas.Any(mp => mp.marcaId == marcaId)
                            && p.deletedAt == null)
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetByPrecioRangeAsync(decimal min, decimal max)
        {
            return await _context.Productos
                .Where(p => p.precio >= min && p.precio <= max && p.deletedAt == null)
                .OrderBy(p => p.precio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetOnSaleAsync()
        {
            return await _context.Productos
                .Where(p => p.precioOferta.HasValue && p.precioOferta < p.precio && p.deletedAt == null)
                .OrderBy(p => p.precioOferta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetNewProductsAsync(int days)
        {
            var since = DateTime.Now.AddDays(-days);
            return await _context.Productos
                .Where(p => p.createdAt >= since && p.deletedAt == null)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))

            searchTerm = searchTerm.ToLower();
            return await _context.Productos
                .Where(p => (p.nombre.ToLower().Contains(searchTerm) ||
                            (p.codigoSerie != null && p.codigoSerie.ToLower().Contains(searchTerm)))
                            && p.deletedAt == null)
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Productos>> GetAllWithFullDetailsAsync()
        {
            return await _context.Productos
                .Include(p => p.CategoriasProductos)
                    .ThenInclude(cp => cp.Categorias)
                .Include(p => p.ProductosMarcas)
                    .ThenInclude(mp => mp.Marcas)
                .Include(p => p.ProductosImagenes)
                    .ThenInclude(pi => pi.Imagenes)
                .Include(p => p.ProductoAtributos)
                    .ThenInclude(pa => pa.Atributos)
                .Include(p => p.ProductosEspecificaciones)
                    .ThenInclude(pe => pe.Especificaciones)
                .Include(p => p.ProductosPreguntas)
                    .ThenInclude(pp => pp.Preguntas)
                .Include(p => p.ProductosOfertas)
                    .ThenInclude(po => po.Ofertas)
                .Where(p => p.deletedAt == null)
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        public async Task<Productos?> GetWithFullDetailsByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.CategoriasProductos)
                    .ThenInclude(cp => cp.Categorias)
                .Include(p => p.ProductosMarcas)
                    .ThenInclude(mp => mp.Marcas)
                .Include(p => p.ProductosImagenes)
                    .ThenInclude(pi => pi.Imagenes)
                .Include(p => p.ProductoAtributos)
                    .ThenInclude(pa => pa.Atributos)
                .Include(p => p.ProductosEspecificaciones)
                    .ThenInclude(pe => pe.Especificaciones)
                .Include(p => p.ProductosPreguntas)
                    .ThenInclude(pp => pp.Preguntas)
                .Include(p => p.ProductosOfertas)
                    .ThenInclude(po => po.Ofertas)
                .Include(p => p.CarritoProductos)
                .Include(p => p.ItemsPedidoProductos)
                .FirstOrDefaultAsync(p => p.id == id && p.deletedAt == null);
        }

        public async Task<Productos?> GetWithCategoriasMarcasAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.CategoriasProductos)
                    .ThenInclude(cp => cp.Categorias)
                .Include(p => p.ProductosMarcas)
                    .ThenInclude(mp => mp.Marcas)
                .FirstOrDefaultAsync(p => p.id == id && p.deletedAt == null);
        }

        //public async Task<IEnumerable<Productos>> GetTopSellingAsync(int top)
        //{
        //    return await _context.Productos
        //        .Include(p => p.ItemsPedidoProductos)
        //        .Where(p => p.deletedAt == null && p.ItemsPedidoProductos.Any())
        //        .OrderByDescending(p => p.ItemsPedidoProductos.Sum(ip => ip.cantidad))
        //        .Take(top)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Productos>> GetRelatedProductsAsync(int productId)
        {
            var producto = await GetWithFullDetailsByIdAsync(productId);
            if (producto == null)
                return Enumerable.Empty<Productos>();

            var categoriaIds = producto.CategoriasProductos.Select(cp => cp.categoriaId).ToList();

            return await _context.Productos
                .Include(p => p.CategoriasProductos)
                .Include(p => p.ProductosImagenes)
                    .ThenInclude(pi => pi.Imagenes)
                .Where(p => p.id != productId
                            && p.deletedAt == null
                            && p.CategoriasProductos.Any(cp => categoriaIds.Contains(cp.categoriaId)))
                .Take(10)
                .ToListAsync();
        }

        public async Task<decimal> GetAveragePriceAsync()
        {
            return await _context.Productos
                .Where(p => p.deletedAt == null)
                .AverageAsync(p => p.precio);
        }

        public async Task<decimal> GetMinPriceAsync()
        {
            return await _context.Productos
                .Where(p => p.deletedAt == null)
                .MinAsync(p => p.precio);
        }

        public async Task<decimal> GetMaxPriceAsync()
        {
            return await _context.Productos
                .Where(p => p.deletedAt == null)
                .MaxAsync(p => p.precio);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Productos
                .CountAsync(p => p.deletedAt == null);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null || producto.deletedAt != null)
                    return false;

                producto.deletedAt = DateTime.Now;
                producto.updatedAt = DateTime.Now;
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
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null || producto.deletedAt == null)
                    return false;

                producto.deletedAt = null;
                producto.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}