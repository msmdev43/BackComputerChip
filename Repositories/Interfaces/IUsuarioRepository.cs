using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuarios>
    {
        Task<Usuarios?> GetByEmailAsync(string email);
        Task<Usuarios?> GetByEmailWithIncludesAsync(string email);
        Task<Usuarios?> GetWithFullDetailsAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllActiveAsync();
        Task<IEnumerable<Usuarios>> GetUsersWithGoogleLoginAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<Usuarios?> GetUserWithPedidosAsync(int id);
        Task<Usuarios?> GetUserWithCarritoAsync(int id);
        Task<Usuarios?> GetUserWithLoginGoogleAsync(int id);
        Task<int> GetTotalActiveUsersAsync();
        Task<IEnumerable<Usuarios>> GetRecentUsersAsync(int days);
        Task<IEnumerable<Usuarios>> SearchUsersAsync(string searchTerm);
        Task<bool> UpdateUserDataAsync(int id, string nombreCompleto, string celular);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}
