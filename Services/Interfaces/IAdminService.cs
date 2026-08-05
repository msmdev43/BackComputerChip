using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IAdminService
    {
        // Autenticación
        Task<Admin?> AuthenticateAsync(string usuario, string password);
        Task<Admin?> GetByUsuarioAsync(string usuario);

        // Dashboard - Estadísticas
        Task<int> GetTotalPedidosAsync();
        Task<int> GetPedidosHoyAsync();
        Task<int> GetCantidadPedidos();
        Task<int> GetTotalUsuariosAsync();
        Task<int> GetTotalCategoriasAsync();
        Task<int> GetCantidadProductosPorCategoriaAsync();
    }
}