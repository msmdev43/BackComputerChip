using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Categorias?> GetByIdAsync(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task<Categorias?> GetByNameAsync(string nombre)
        {
            return await _categoriaRepository.GetByNameAsync(nombre);
        }

        public async Task<Categorias?> GetWithProductosAsync(int id)
        {
            return await _categoriaRepository.GetWithProductosAsync(id);
        }

        public async Task<IEnumerable<Categorias>> GetAllActiveAsync()
        {
            return await _categoriaRepository.GetAllActiveAsync();
        }

        public async Task<IEnumerable<Categorias>> GetAllWithProductosAsync()
        {
            return await _categoriaRepository.GetAllWithProductosAsync();
        }

        public async Task<int> GetProductCountByCategoriaAsync(int categoriaId)
        {
            return await _categoriaRepository.GetProductCountByCategoriaAsync(categoriaId);
        }

        public async Task<bool> ExistsByNameAsync(string nombre)
        {
            return await _categoriaRepository.ExistsByNameAsync(nombre);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Categorias> CreateCategoriaAsync(string nombre)
        {
            if (await _categoriaRepository.ExistsByNameAsync(nombre))
                throw new InvalidOperationException($"La categoría '{nombre}' ya existe");

            var categoria = new Categorias
            {
                nombre = nombre,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            await _categoriaRepository.AddAsync(categoria);
            await _categoriaRepository.SaveChangesAsync();

            return categoria;
        }

        public async Task<bool> UpdateCategoriaAsync(int id, string nombre)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null || categoria.deletedAt != null)
                    return false;

                // Verificar que el nombre no esté en uso por otra categoría
                var existing = await _categoriaRepository.GetByNameAsync(nombre);
                if (existing != null && existing.id != id)
                    return false;

                categoria.nombre = nombre;
                categoria.updatedAt = DateTime.Now;

                _categoriaRepository.Update(categoria);
                return await _categoriaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteCategoriaAsync(int id)
        {
            return await _categoriaRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RestoreCategoriaAsync(int id)
        {
            return await _categoriaRepository.RestoreAsync(id);
        }

        public async Task<bool> DeleteCategoriaPermanentlyAsync(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null)
                    return false;

                _categoriaRepository.Remove(categoria);
                return await _categoriaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}