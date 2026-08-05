using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class ZonaEnvioService : IZonaEnvioService
    {
        private readonly IZonaEnvioRepository _zonaEnvioRepository;

        public ZonaEnvioService(IZonaEnvioRepository zonaEnvioRepository)
        {
            _zonaEnvioRepository = zonaEnvioRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<ZonaEnvio?> GetByIdAsync(int id)
        {
            return await _zonaEnvioRepository.GetByIdAsync(id);
        }

        public async Task<ZonaEnvio?> GetByCodigoPostalAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return null;

            return await _zonaEnvioRepository.GetByCodigoPostalAsync(codigoPostal);
        }

        public async Task<ZonaEnvio?> GetByCiudadProvinciaAsync(string ciudad, string provincia)
        {
            if (string.IsNullOrWhiteSpace(ciudad) || string.IsNullOrWhiteSpace(provincia))
                return null;

            return await _zonaEnvioRepository.GetByCiudadProvinciaAsync(ciudad, provincia);
        }

        public async Task<IEnumerable<ZonaEnvio>> GetByPaisAsync(string pais)
        {
            if (string.IsNullOrWhiteSpace(pais))
                return new List<ZonaEnvio>();

            return await _zonaEnvioRepository.GetByPaisAsync(pais);
        }

        public async Task<IEnumerable<ZonaEnvio>> GetAllActiveAsync()
        {
            return await _zonaEnvioRepository.GetAllActiveAsync();
        }

        public async Task<decimal> GetCostoEnvioAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return 0;

            return await _zonaEnvioRepository.GetCostoEnvioAsync(codigoPostal);
        }

        public async Task<bool> ExistsByCodigoPostalAsync(string codigoPostal)
        {
            return await _zonaEnvioRepository.ExistsByCodigoPostalAsync(codigoPostal);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<ZonaEnvio> CreateZonaEnvioAsync(string ciudad, string provincia, string pais, string costo, string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(ciudad))
                throw new ArgumentException("La ciudad no puede estar vacía");
            if (string.IsNullOrWhiteSpace(provincia))
                throw new ArgumentException("La provincia no puede estar vacía");
            if (string.IsNullOrWhiteSpace(pais))
                throw new ArgumentException("El país no puede estar vacío");
            if (string.IsNullOrWhiteSpace(codigoPostal))
                throw new ArgumentException("El código postal no puede estar vacío");

            if (await _zonaEnvioRepository.ExistsByCodigoPostalAsync(codigoPostal))
                throw new InvalidOperationException($"El código postal '{codigoPostal}' ya está registrado");

            var zonaEnvio = new ZonaEnvio
            {
                ciudad = ciudad,
                provincia = provincia,
                pais = pais,
                costo = costo,
                codigoPostal = codigoPostal
            };

            await _zonaEnvioRepository.AddAsync(zonaEnvio);
            await _zonaEnvioRepository.SaveChangesAsync();

            return zonaEnvio;
        }

        public async Task<bool> UpdateZonaEnvioAsync(int id, string ciudad, string provincia, string pais, string costo, string codigoPostal)
        {
            try
            {
                var zonaEnvio = await _zonaEnvioRepository.GetByIdAsync(id);
                if (zonaEnvio == null)
                    return false;

                // Verificar que el código postal no esté en uso por otra zona
                if (!string.IsNullOrWhiteSpace(codigoPostal))
                {
                    var existing = await _zonaEnvioRepository.GetByCodigoPostalAsync(codigoPostal);
                    if (existing != null && existing.id != id)
                        return false;
                }

                zonaEnvio.ciudad = ciudad ?? zonaEnvio.ciudad;
                zonaEnvio.provincia = provincia ?? zonaEnvio.provincia;
                zonaEnvio.pais = pais ?? zonaEnvio.pais;
                zonaEnvio.costo = costo ?? zonaEnvio.costo;
                zonaEnvio.codigoPostal = codigoPostal ?? zonaEnvio.codigoPostal;

                _zonaEnvioRepository.Update(zonaEnvio);
                return await _zonaEnvioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteZonaEnvioAsync(int id)
        {
            try
            {
                var zonaEnvio = await _zonaEnvioRepository.GetByIdAsync(id);
                if (zonaEnvio == null)
                    return false;

                _zonaEnvioRepository.Remove(zonaEnvio);
                return await _zonaEnvioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCostoEnvioAsync(int id, decimal costo)
        {
            try
            {
                var zonaEnvio = await _zonaEnvioRepository.GetByIdAsync(id);
                if (zonaEnvio == null)
                    return false;

                zonaEnvio.costo = costo.ToString("F2");
                _zonaEnvioRepository.Update(zonaEnvio);
                return await _zonaEnvioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}