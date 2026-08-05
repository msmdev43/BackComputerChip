using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMarcaRepository _marcaRepository;

        public ProductoService(
            IProductoRepository productoRepository,
            ICategoriaRepository categoriaRepository,
            IMarcaRepository marcaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
            _marcaRepository = marcaRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Productos?> GetByIdAsync(int id)
        {
            return await _productoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Productos>> GetAllActiveAsync()
        {
            return await _productoRepository.GetAllWithFullDetailsAsync();
        }

        public async Task<IEnumerable<Productos>> GetByCategoriaAsync(int categoriaId)
        {
            return await _productoRepository.GetByCategoriaAsync(categoriaId);
        }

        public async Task<IEnumerable<Productos>> GetByMarcaAsync(int marcaId)
        {
            return await _productoRepository.GetByMarcaAsync(marcaId);
        }

        public async Task<IEnumerable<Productos>> GetByPrecioRangeAsync(decimal min, decimal max)
        {
            return await _productoRepository.GetByPrecioRangeAsync(min, max);
        }

        public async Task<IEnumerable<Productos>> GetInStockAsync()
        {
            return await _productoRepository.GetInStockAsync();
        }

        public async Task<IEnumerable<Productos>> GetOutOfStockAsync()
        {
            return await _productoRepository.GetOutOfStockAsync();
        }

        public async Task<IEnumerable<Productos>> GetOnSaleAsync()
        {
            return await _productoRepository.GetOnSaleAsync();
        }

        public async Task<IEnumerable<Productos>> GetNewProductsAsync(int days)
        {
            return await _productoRepository.GetNewProductsAsync(days);
        }

        public async Task<IEnumerable<Productos>> SearchProductsAsync(string searchTerm)
        {
            return await _productoRepository.SearchProductsAsync(searchTerm);
        }

        public async Task<Productos?> GetProductWithFullDetailsAsync(int id)
        {
            return await _productoRepository.GetWithFullDetailsByIdAsync(id);
        }

        public async Task<IEnumerable<Productos>> GetRelatedProductsAsync(int productId)
        {
            return await _productoRepository.GetRelatedProductsAsync(productId);
        }

        public async Task<decimal> GetAveragePriceAsync()
        {
            return await _productoRepository.GetAveragePriceAsync();
        }

        public async Task<decimal> GetMinPriceAsync()
        {
            return await _productoRepository.GetMinPriceAsync();
        }

        public async Task<decimal> GetMaxPriceAsync()
        {
            return await _productoRepository.GetMaxPriceAsync();
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _productoRepository.GetTotalProductsAsync();
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Productos> CreateProductAsync(Productos producto, List<int> categoriaIds, List<int> marcaIds)
        {
            producto.createdAt = DateTime.Now;
            producto.updatedAt = DateTime.Now;

            await _productoRepository.AddAsync(producto);
            await _productoRepository.SaveChangesAsync();

            // Agregar categorías
            if (categoriaIds != null && categoriaIds.Any())
            {
                await AddCategoriesToProductAsync(producto.id, categoriaIds);
            }

            // Agregar marcas
            if (marcaIds != null && marcaIds.Any())
            {
                await AddBrandsToProductAsync(producto.id, marcaIds);
            }

            return producto;
        }

        public async Task<bool> UpdateProductAsync(int id, Productos producto)
        {
            try
            {
                var existing = await _productoRepository.GetByIdAsync(id);
                if (existing == null || existing.deletedAt != null)
                    return false;

                // Actualizar solo los campos permitidos
                existing.nombre = producto.nombre;
                existing.precio = producto.precio;
                existing.precioOferta = producto.precioOferta;
                existing.garantia = producto.garantia;
                existing.stock = producto.stock;
                existing.envioGratis = producto.envioGratis;
                existing.codigoSerie = producto.codigoSerie;
                existing.updatedAt = DateTime.Now;

                _productoRepository.Update(existing);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStockAsync(int id, bool stock)
        {
            return await _productoRepository.UpdateStockAsync(id, stock);
        }

        public async Task<bool> SoftDeleteProductAsync(int id)
        {
            return await _productoRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RestoreProductAsync(int id)
        {
            return await _productoRepository.RestoreAsync(id);
        }

        public async Task<bool> DeleteProductPermanentlyAsync(int id)
        {
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);
                if (producto == null)
                    return false;

                _productoRepository.Remove(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddCategoriesToProductAsync(int productoId, List<int> categoriaIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                foreach (var categoriaId in categoriaIds)
                {
                    if (!producto.CategoriasProductos.Any(cp => cp.categoriaId == categoriaId))
                    {
                        producto.CategoriasProductos.Add(new CategoriasProductos
                        {
                            productoId = productoId,
                            categoriaId = categoriaId
                        });
                    }
                }

                _productoRepository.Update(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveCategoriesFromProductAsync(int productoId, List<int> categoriaIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var toRemove = producto.CategoriasProductos
                    .Where(cp => categoriaIds.Contains(cp.categoriaId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    producto.CategoriasProductos.Remove(item);
                }

                _productoRepository.Update(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddBrandsToProductAsync(int productoId, List<int> marcaIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                foreach (var marcaId in marcaIds)
                {
                    if (!producto.ProductosMarcas.Any(mp => mp.marcaId == marcaId))
                    {
                        producto.ProductosMarcas.Add(new ProductosMarcas
                        {
                            productoId = productoId,
                            marcaId = marcaId
                        });
                    }
                }

                _productoRepository.Update(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveBrandsFromProductAsync(int productoId, List<int> marcaIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var toRemove = producto.ProductosMarcas
                    .Where(mp => marcaIds.Contains(mp.marcaId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    producto.ProductosMarcas.Remove(item);
                }

                _productoRepository.Update(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}