using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin?> GetByUsuarioAsync(string usuario);
        Task<int> GetTotalPedidosAsync();
        Task<int> GetPedidosHoyAsync();
        Task<int> GetCantidadPedidos();
        Task<int> GetTotalUsuariosAsync();
        Task<int> GetTotalCategoriasAsync();
        Task<int> GetCantidadProductosPorCategoriaAsync();
    }
}