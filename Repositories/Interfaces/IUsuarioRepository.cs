using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuarios>
    {
        Task<Usuarios> GetByEmailAsync(string email);
        Task<Usuarios> GetByEmailWithIncludesAsync(string email);
        Task<Usuarios> GetWithFullDetailsAsync(int id);
        Task<IEnumerable<Usuarios>> GetActiveUsersAsync();
        Task<IEnumerable<Usuarios>> GetUsersWithGoogleLoginAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<Usuarios> GetUserWithPedidosAsync(int id);
        Task<Usuarios> GetUserWithCarritoAsync(int id);
        Task<int> GetTotalActiveUsersAsync();
        Task<IEnumerable<Usuarios>> GetRecentUsersAsync(int days);
    }
}
