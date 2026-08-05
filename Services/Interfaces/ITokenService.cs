using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface ITokenService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<SantanderToken?> GetByUsuarioAsync(int usuarioId);
        Task<SantanderToken?> GetValidTokenAsync(int usuarioId);
        Task<bool> TokenExistsAsync(int usuarioId);
        Task<bool> IsTokenExpiredAsync(int usuarioId);
        Task<SantanderToken?> GetByAccessTokenAsync(string accessToken);
        Task<SantanderToken?> GetByRefreshTokenAsync(string refreshToken);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<bool> SaveTokenAsync(int usuarioId, string accessToken, string refreshToken, int expiresIn);
        Task<bool> RefreshTokenAsync(int usuarioId, string newAccessToken, string newRefreshToken, int expiresIn);
        Task<bool> RevokeTokenAsync(int usuarioId);

        // ============================================
        // OPERACIONES AUXILIARES
        // ============================================
        Task<string> GenerateAccessTokenAsync(int usuarioId);
        Task<string> GenerateRefreshTokenAsync();
        Task<bool> ValidateAccessTokenAsync(string token);
    }
}