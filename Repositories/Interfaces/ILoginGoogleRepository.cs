using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface ILoginGoogleRepository : IRepository<LoginGoogle>
    {
        Task<LoginGoogle?> GetByGoogleSubAsync(string googleSub);
        Task<LoginGoogle?> GetByUsuarioIdAsync(int usuarioId);
        Task<LoginGoogle?> GetWithUsuarioAsync(int id);
        Task<LoginGoogle?> GetByGoogleSubWithUsuarioAsync(string googleSub);
        Task<bool> ExistsByGoogleSubAsync(string googleSub);
        Task<bool> ExistsByUsuarioIdAsync(int usuarioId);
        Task UpdateLastLoginAsync(int id);
        Task<bool> SoftDeleteByUsuarioIdAsync(int usuarioId);
        Task<LoginGoogle?> GetActiveByUsuarioIdAsync(int usuarioId);
    }
}
