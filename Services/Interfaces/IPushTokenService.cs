using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IPushTokenService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<IEnumerable<PushToken>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<PushToken>> GetByAdminAsync(int adminId);
        Task<PushToken?> GetByTokenAsync(string token);
        Task<IEnumerable<PushToken>> GetByDispositivoAsync(string dispositivo);
        Task<IEnumerable<PushToken>> GetActiveUserTokensAsync();
        Task<IEnumerable<PushToken>> GetActiveAdminTokensAsync();
        Task<bool> TokenExistsAsync(string token);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        Task<bool> RegisterUserTokenAsync(int usuarioId, string token, string dispositivo);
        Task<bool> RegisterAdminTokenAsync(int adminId, string token, string dispositivo);
        Task<bool> RemoveTokenAsync(string token);
        Task<bool> RemoveAllUserTokensAsync(int usuarioId);
        Task<bool> RemoveAllAdminTokensAsync(int adminId);
        Task<IEnumerable<string>> GetUserTokensForNotificationAsync(int? usuarioId = null);
        Task<IEnumerable<string>> GetAdminTokensForNotificationAsync(int? adminId = null);
        Task<bool> SendPushToUserAsync(int usuarioId, string titulo, string mensaje, object? data = null);
        Task<int> SendPushToAllUsersAsync(string titulo, string mensaje, object? data = null);
        Task<int> SendPushToAllAdminsAsync(string titulo, string mensaje, object? data = null);
        Task<int> SendPushToUserListAsync(IEnumerable<int> usuarioIds, string titulo, string mensaje, object? data = null);
    }
}