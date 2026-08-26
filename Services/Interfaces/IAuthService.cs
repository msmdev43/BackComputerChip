using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IAuthService
    {
        // ============================================
        // REGISTRO Y LOGIN
        // ============================================

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        Task<(Usuarios Usuario, string AccessToken, string RefreshToken)> RegisterAsync(
            string nombreCompleto,
            string email,
            string password,
            string? pais = null,
            string? provincia = null,
            string? ciudad = null,
            string? calle = null,
            string? numero = null,
            string? celular = null);

        /// <summary>
        /// Login de usuario con email y password
        /// </summary>
        Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginAsync(string email, string password);

        /// <summary>
        /// Login con Google
        /// </summary>
        Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginGoogleAsync(
            string googleSub,
            string email,
            string nombre,
            string? avatarUrl,
            string? refreshToken);

        /// <summary>
        /// Login de administrador
        /// </summary>
        Task<(Admin Admin, string AccessToken, string RefreshToken)?> AdminLoginAsync(string usuario, string password);

        // ============================================
        // TOKENS
        // ============================================

        /// <summary>
        /// Renueva el access token usando un refresh token
        /// </summary>
        Task<string?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revoca un token (logout)
        /// </summary>
        Task<bool> LogoutAsync(string refreshToken);

        /// <summary>
        /// Revoca todos los tokens de un usuario (logout all)
        /// </summary>
        Task<bool> LogoutAllAsync(int usuarioId);

        /// <summary>
        /// Verifica un access token
        /// </summary>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// Obtiene el ID de usuario desde un token
        /// </summary>
        Task<int?> GetUsuarioIdFromTokenAsync(string token);

        // ============================================
        // VERIFICACIÓN DE EMAIL
        // ============================================

        /// <summary>
        /// Verifica el email de un usuario
        /// </summary>
        Task<bool> VerifyEmailAsync(int usuarioId);

        /// <summary>
        /// Envía un email de verificación
        /// </summary>
        Task<bool> SendVerificationEmailAsync(int usuarioId);
    }
}