using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface ILoginGoogleService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<LoginGoogle?> GetByGoogleSubAsync(string googleSub);
        Task<LoginGoogle?> GetByUsuarioIdAsync(int usuarioId);
        Task<LoginGoogle?> GetByGoogleSubWithUsuarioAsync(string googleSub);
        Task<bool> ExistsByGoogleSubAsync(string googleSub);
        Task<bool> ExistsByUsuarioIdAsync(int usuarioId);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<LoginGoogle> CreateOrUpdateGoogleLoginAsync(string googleSub, string email, string nombre, string avatarUrl, string? refreshToken);
        Task<bool> UpdateLastLoginAsync(int id);
        Task<bool> UpdateRefreshTokenAsync(int id, string refreshToken);
        Task<bool> SoftDeleteByUsuarioIdAsync(int usuarioId);
    }
}