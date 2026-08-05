using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IUsuarioService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Usuarios?> GetByIdAsync(int id);
        Task<Usuarios?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuarios>> GetAllActiveAsync();
        Task<IEnumerable<Usuarios>> GetUsersWithGoogleLoginAsync();
        Task<Usuarios?> GetUserWithFullDetailsAsync(int id);
        Task<IEnumerable<Usuarios>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<Usuarios>> GetRecentUsersAsync(int days);
        Task<int> GetTotalActiveUsersAsync();
        Task<bool> ExistsByEmailAsync(string email);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Usuarios> CreateUserAsync(Usuarios usuario, string? password = null);
        Task<bool> UpdateUserAsync(int id, string nombreCompleto, string celular);
        Task<bool> UpdateUserEmailAsync(int id, string email);
        Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<bool> SoftDeleteUserAsync(int id);
        Task<bool> RestoreUserAsync(int id);
        Task<bool> DeleteUserPermanentlyAsync(int id);

        // ============================================
        // OPERACIONES DE AUTENTICACIÓN
        // ============================================
        Task<Usuarios?> AuthenticateAsync(string email, string password);
        Task<bool> VerifyEmailAsync(int id);
        Task<bool> SendPasswordResetEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}