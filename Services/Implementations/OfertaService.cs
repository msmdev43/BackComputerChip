using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository _ofertaRepository;
        private readonly IProductoRepository _productoRepository;

        public OfertaService(
            IOfertaRepository ofertaRepository,
            IProductoRepository productoRepository)
        {
            _ofertaRepository = ofertaRepository;
            _productoRepository = productoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Ofertas?> GetByIdAsync(int id)
        {
            return await _ofertaRepository.GetByIdAsync(id);
        }

        public async Task<Ofertas?> GetWithProductosAsync(int id)
        {
            return await _ofertaRepository.GetWithProductosAsync(id);
        }

        public async Task<IEnumerable<Ofertas>> GetAllActiveAsync()
        {
            return await _ofertaRepository.GetAllActiveAsync();
        }

        public async Task<IEnumerable<Ofertas>> GetByTipoAsync(string tipoOferta)
        {
            return await _ofertaRepository.GetByTipoAsync(tipoOferta);
        }

        public async Task<IEnumerable<Ofertas>> GetAllWithProductosAsync()
        {
            return await _ofertaRepository.GetAllWithProductosAsync();
        }

        public async Task<IEnumerable<Ofertas>> GetOfertasVigentesAsync()
        {
            return await _ofertaRepository.GetOfertasVigentesAsync();
        }

        public async Task<IEnumerable<Ofertas>> GetByDescuentoMayorAsync(decimal descuentoMinimo)
        {
            return await _ofertaRepository.GetByDescuentoMayorAsync(descuentoMinimo);
        }

        public async Task<decimal> GetMaxDescuentoAsync()
        {
            return await _ofertaRepository.GetMaxDescuentoAsync();
        }

        public async Task<bool> HasActiveOfertaForProductoAsync(int productoId)
        {
            return await _ofertaRepository.HasActiveOfertaForProductoAsync(productoId);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Ofertas> CreateOfertaAsync(Ofertas oferta, List<int> productoIds)
        {
            oferta.createdAt = DateTime.Now;
            oferta.updatedAt = DateTime.Now;

            await _ofertaRepository.AddAsync(oferta);
            await _ofertaRepository.SaveChangesAsync();

            if (productoIds != null && productoIds.Any())
            {
                await AddProductsToOfertaAsync(oferta.id, productoIds);
            }

            return oferta;
        }

        public async Task<bool> UpdateOfertaAsync(int id, Ofertas oferta)
        {
            try
            {
                var existing = await _ofertaRepository.GetByIdAsync(id);
                if (existing == null || existing.deletedAt != null)
                    return false;

                existing.titulo = oferta.titulo;
                existing.subtitulo = oferta.subtitulo;
                existing.tipoOferta = oferta.tipoOferta;
                existing.tipoDescuento = oferta.tipoDescuento;
                existing.descuento = oferta.descuento;
                existing.precioOriginal = oferta.precioOriginal;
                existing.precioOferta = oferta.precioOferta;
                existing.updatedAt = DateTime.Now;

                _ofertaRepository.Update(existing);
                return await _ofertaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddProductsToOfertaAsync(int ofertaId, List<int> productoIds)
        {
            try
            {
                var oferta = await _ofertaRepository.GetWithProductosAsync(ofertaId);
                if (oferta == null)
                    return false;

                foreach (var productoId in productoIds)
                {
                    if (!oferta.ProductosOfertas.Any(po => po.productoId == productoId))
                    {
                        oferta.ProductosOfertas.Add(new ProductosOfertas
                        {
                            ofertaId = ofertaId,
                            productoId = productoId
                        });
                    }
                }

                _ofertaRepository.Update(oferta);
                return await _ofertaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveProductsFromOfertaAsync(int ofertaId, List<int> productoIds)
        {
            try
            {
                var oferta = await _ofertaRepository.GetWithProductosAsync(ofertaId);
                if (oferta == null)
                    return false;

                var toRemove = oferta.ProductosOfertas
                    .Where(po => productoIds.Contains(po.productoId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    oferta.ProductosOfertas.Remove(item);
                }

                _ofertaRepository.Update(oferta);
                return await _ofertaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteOfertaAsync(int id)
        {
            return await _ofertaRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RestoreOfertaAsync(int id)
        {
            return await _ofertaRepository.RestoreAsync(id);
        }

        public async Task<bool> DeleteOfertaPermanentlyAsync(int id)
        {
            try
            {
                var oferta = await _ofertaRepository.GetByIdAsync(id);
                if (oferta == null)
                    return false;

                _ofertaRepository.Remove(oferta);
                return await _ofertaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ApplyOfertaToProductAsync(int productoId, int ofertaId)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var oferta = await _ofertaRepository.GetByIdAsync(ofertaId);
                if (oferta == null)
                    return false;

                // Remover oferta anterior si existe
                if (producto.precioOferta.HasValue)
                {
                    await RemoveOfertaFromProductAsync(productoId);
                }

                // Aplicar nueva oferta
                producto.precioOferta = oferta.precioOferta;
                producto.updatedAt = DateTime.Now;

                _productoRepository.Update(producto);
                return await _productoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveOfertaFromProductAsync(int productoId)
        {
            try
            {
                var producto = await _productoRepository.GetByIdAsync(productoId);
                if (producto == null)
                    return false;

                producto.precioOferta = null;
                producto.updatedAt = DateTime.Now;

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