using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IPushTokenRepository : IRepository<PushToken>
    {
        // ============================================
        // MÉTODOS PARA PUSH TOKEN
        // ============================================

        /// <summary>
        /// Obtiene todos los tokens de un usuario
        /// </summary>
        Task<IEnumerable<PushToken>> GetByUsuarioAsync(int usuarioId);

        /// <summary>
        /// Obtiene todos los tokens de un admin
        /// </summary>
        Task<IEnumerable<PushToken>> GetByAdminAsync(int adminId);

        /// <summary>
        /// Obtiene un token específico
        /// </summary>
        Task<PushToken?> GetByTokenAsync(string token);

        /// <summary>
        /// Obtiene tokens por dispositivo
        /// </summary>
        Task<IEnumerable<PushToken>> GetByDispositivoAsync(string dispositivo);

        /// <summary>
        /// Obtiene todos los tokens de usuarios activos
        /// </summary>
        Task<IEnumerable<PushToken>> GetActiveUserTokensAsync();

        /// <summary>
        /// Obtiene todos los tokens de admins activos
        /// </summary>
        Task<IEnumerable<PushToken>> GetActiveAdminTokensAsync();

        /// <summary>
        /// Registra un nuevo token para usuario
        /// </summary>
        Task<bool> RegisterUserTokenAsync(int usuarioId, string token, string dispositivo);

        /// <summary>
        /// Registra un nuevo token para admin
        /// </summary>
        Task<bool> RegisterAdminTokenAsync(int adminId, string token, string dispositivo);

        /// <summary>
        /// Elimina un token específico
        /// </summary>
        Task<bool> RemoveTokenAsync(string token);

        /// <summary>
        /// Elimina todos los tokens de un usuario
        /// </summary>
        Task<bool> RemoveAllUserTokensAsync(int usuarioId);

        /// <summary>
        /// Elimina todos los tokens de un admin
        /// </summary>
        Task<bool> RemoveAllAdminTokensAsync(int adminId);

        /// <summary>
        /// Verifica si un token ya está registrado
        /// </summary>
        Task<bool> TokenExistsAsync(string token);

        /// <summary>
        /// Obtiene los tokens para enviar notificaciones a usuarios
        /// </summary>
        Task<IEnumerable<string>> GetUserTokensForNotificationAsync(int? usuarioId = null);

        /// <summary>
        /// Obtiene los tokens para enviar notificaciones a admins
        /// </summary>
        Task<IEnumerable<string>> GetAdminTokensForNotificationAsync(int? adminId = null);
    }
}