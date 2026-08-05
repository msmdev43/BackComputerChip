using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin?> AuthenticateAsync(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                return null;

            var admin = await _adminRepository.GetByUsuarioAsync(usuario);
            if (admin == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, admin.password))
                return null;

            return admin;
        }

        public async Task<Admin?> GetByUsuarioAsync(string usuario)
        {
            return await _adminRepository.GetByUsuarioAsync(usuario);
        }

        public async Task<int> GetTotalPedidosAsync()
        {
            return await _adminRepository.GetTotalPedidosAsync();
        }

        public async Task<int> GetPedidosHoyAsync()
        {
            return await _adminRepository.GetPedidosHoyAsync();
        }

        public async Task<int> GetCantidadPedidos()
        {
            return await _adminRepository.GetCantidadPedidos();
        }

        public async Task<int> GetTotalUsuariosAsync()
        {
            return await _adminRepository.GetTotalUsuariosAsync();
        }

        public async Task<int> GetTotalCategoriasAsync()
        {
            return await _adminRepository.GetTotalCategoriasAsync();
        }

        public async Task<int> GetCantidadProductosPorCategoriaAsync()
        {
            return await _adminRepository.GetCantidadProductosPorCategoriaAsync();
        }
    }
}