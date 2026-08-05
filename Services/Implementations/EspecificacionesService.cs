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
    public class EspecificacionesService : IEspecificacionesService
    {
        private readonly IEspecificacionesRepository _especificacionesRepository;
        private readonly IProductoRepository _productoRepository;

        public EspecificacionesService(
            IEspecificacionesRepository especificacionesRepository,
            IProductoRepository productoRepository)
        {
            _especificacionesRepository = especificacionesRepository;
            _productoRepository = productoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Especificaciones?> GetByIdAsync(int id)
        {
            return await _especificacionesRepository.GetByIdAsync(id);
        }

        public async Task<Especificaciones?> GetByTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return null;

            return await _especificacionesRepository.GetByTituloAsync(titulo);
        }

        public async Task<IEnumerable<Especificaciones>> GetByProductoAsync(int productoId)
        {
            return await _especificacionesRepository.GetByProductoAsync(productoId);
        }

        public async Task<IEnumerable<Especificaciones>> GetAllWithProductosAsync()
        {
            return await _especificacionesRepository.GetAllWithProductosAsync();
        }

        public async Task<Especificaciones?> GetWithProductosByIdAsync(int id)
        {
            return await _especificacionesRepository.GetWithProductosByIdAsync(id);
        }

        public async Task<bool> ExistsByTituloAsync(string titulo)
        {
            return await _especificacionesRepository.ExistsByTituloAsync(titulo);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Especificaciones> CreateEspecificacionAsync(string titulo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título de la especificación no puede estar vacío");

            if (await _especificacionesRepository.ExistsByTituloAsync(titulo))
                throw new InvalidOperationException($"La especificación '{titulo}' ya existe");

            var especificacion = new Especificaciones
            {
                titulo = titulo,
                descripcion = descripcion
            };

            await _especificacionesRepository.AddAsync(especificacion);
            await _especificacionesRepository.SaveChangesAsync();

            return especificacion;
        }

        public async Task<bool> UpdateEspecificacionAsync(int id, string titulo, string descripcion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                    return false;

                var especificacion = await _especificacionesRepository.GetByIdAsync(id);
                if (especificacion == null)
                    return false;

                // Verificar que el título no esté en uso por otra especificación
                var existing = await _especificacionesRepository.GetByTituloAsync(titulo);
                if (existing != null && existing.id != id)
                    return false;

                especificacion.titulo = titulo;
                especificacion.descripcion = descripcion;

                _especificacionesRepository.Update(especificacion);
                return await _especificacionesRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEspecificacionAsync(int id)
        {
            try
            {
                var especificacion = await _especificacionesRepository.GetByIdAsync(id);
                if (especificacion == null)
                    return false;

                _especificacionesRepository.Remove(especificacion);
                return await _especificacionesRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddToProductAsync(int especificacionId, int productoId)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var especificacion = await _especificacionesRepository.GetByIdAsync(especificacionId);
                if (especificacion == null)
                    return false;

                if (!producto.ProductosEspecificaciones.Any(pe => pe.especificacionId == especificacionId))
                {
                    producto.ProductosEspecificaciones.Add(new ProductosEspecificaciones
                    {
                        productoId = productoId,
                        especificacionId = especificacionId
                    });

                    _productoRepository.Update(producto);
                    return await _productoRepository.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromProductAsync(int especificacionId, int productoId)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var toRemove = producto.ProductosEspecificaciones
                    .FirstOrDefault(pe => pe.especificacionId == especificacionId);

                if (toRemove != null)
                {
                    producto.ProductosEspecificaciones.Remove(toRemove);
                    _productoRepository.Update(producto);
                    return await _productoRepository.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddMultipleToProductAsync(int productoId, List<int> especificacionIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                foreach (var especificacionId in especificacionIds)
                {
                    if (!producto.ProductosEspecificaciones.Any(pe => pe.especificacionId == especificacionId))
                    {
                        producto.ProductosEspecificaciones.Add(new ProductosEspecificaciones
                        {
                            productoId = productoId,
                            especificacionId = especificacionId
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

        public async Task<bool> RemoveMultipleFromProductAsync(int productoId, List<int> especificacionIds)
        {
            try
            {
                var producto = await _productoRepository.GetWithFullDetailsByIdAsync(productoId);
                if (producto == null)
                    return false;

                var toRemove = producto.ProductosEspecificaciones
                    .Where(pe => especificacionIds.Contains(pe.especificacionId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    producto.ProductosEspecificaciones.Remove(item);
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