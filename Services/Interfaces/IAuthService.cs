using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IAuthService
    {
        // ============================================
        // LOGIN DE USUARIO CON GOOGLE
        // ============================================
        Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginGoogleAsync(
            string googleSub,
            string email,
            string nombre,
            string? avatarUrl,
            string? refreshToken);

        // ============================================
        // LOGIN DE ADMIN
        // ============================================
        Task<(Admin Admin, string AccessToken, string RefreshToken)?> AdminLoginAsync(
            string usuario,
            string password);

        // ============================================
        // TOKENS
        // ============================================
        Task<string?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> LogoutAllAsync(int usuarioId);
    }
}