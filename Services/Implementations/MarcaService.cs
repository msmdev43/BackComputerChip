using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcaService(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Marcas?> GetByIdAsync(int id)
        {
            return await _marcaRepository.GetByIdAsync(id);
        }

        public async Task<Marcas?> GetByNameAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            return await _marcaRepository.GetByNameAsync(nombre);
        }

        public async Task<Marcas?> GetWithProductosAsync(int id)
        {
            return await _marcaRepository.GetWithProductosAsync(id);
        }

        public async Task<IEnumerable<Marcas>> GetAllActiveAsync()
        {
            return await _marcaRepository.GetAllActiveAsync();
        }

        public async Task<IEnumerable<Marcas>> GetAllWithProductosAsync()
        {
            return await _marcaRepository.GetAllWithProductosAsync();
        }

        public async Task<int> GetProductCountByMarcaAsync(int marcaId)
        {
            return await _marcaRepository.GetProductCountByMarcaAsync(marcaId);
        }

        public async Task<bool> ExistsByNameAsync(string nombre)
        {
            return await _marcaRepository.ExistsByNameAsync(nombre);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Marcas> CreateMarcaAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la marca no puede estar vacío");

            if (await _marcaRepository.ExistsByNameAsync(nombre))
                throw new InvalidOperationException($"La marca '{nombre}' ya existe");

            var marca = new Marcas
            {
                nombre = nombre,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            await _marcaRepository.AddAsync(marca);
            await _marcaRepository.SaveChangesAsync();

            return marca;
        }

        public async Task<bool> UpdateMarcaAsync(int id, string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return false;

                var marca = await _marcaRepository.GetByIdAsync(id);
                if (marca == null || marca.deletedAt != null)
                    return false;

                // Verificar que el nombre no esté en uso por otra marca
                var existing = await _marcaRepository.GetByNameAsync(nombre);
                if (existing != null && existing.id != id)
                    return false;

                marca.nombre = nombre;
                marca.updatedAt = DateTime.Now;

                _marcaRepository.Update(marca);
                return await _marcaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteMarcaAsync(int id)
        {
            return await _marcaRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RestoreMarcaAsync(int id)
        {
            return await _marcaRepository.RestoreAsync(id);
        }

        public async Task<bool> DeleteMarcaPermanentlyAsync(int id)
        {
            try
            {
                var marca = await _marcaRepository.GetByIdAsync(id);
                if (marca == null)
                    return false;

                _marcaRepository.Remove(marca);
                return await _marcaRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}