using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface ITokenRepository : IRepository<SantanderToken>
    {
        // ============================================
        // MÉTODOS PARA SANTANDER TOKEN
        // ============================================

        /// <summary>
        /// Obtiene el token de Santander por usuario
        /// </summary>
        Task<SantanderToken?> GetByUsuarioAsync(int usuarioId);

        /// <summary>
        /// Obtiene el token válido (no expirado) de Santander
        /// </summary>
        Task<SantanderToken?> GetValidTokenAsync(int usuarioId);

        /// <summary>
        /// Verifica si existe un token para el usuario
        /// </summary>
        Task<bool> TokenExistsAsync(int usuarioId);

        /// <summary>
        /// Actualiza o crea un nuevo token de Santander
        /// </summary>
        Task<bool> UpdateTokenAsync(int usuarioId, string accessToken, string refreshToken, int expiresIn);

        /// <summary>
        /// Revoca (elimina) el token de Santander
        /// </summary>
        Task<bool> RevokeTokenAsync(int usuarioId);

        /// <summary>
        /// Verifica si el token ha expirado
        /// </summary>
        Task<bool> IsTokenExpiredAsync(int usuarioId);

        /// <summary>
        /// Obtiene el token con los datos del usuario
        /// </summary>
        Task<SantanderToken?> GetTokenWithUsuarioAsync(int id);

        /// <summary>
        /// Obtiene el token por access token
        /// </summary>
        Task<SantanderToken?> GetByAccessTokenAsync(string accessToken);

        /// <summary>
        /// Obtiene el token por refresh token
        /// </summary>
        Task<SantanderToken?> GetByRefreshTokenAsync(string refreshToken);
    }
}