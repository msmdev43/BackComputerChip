using System;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAdminService _adminService;
        private readonly ITokenService _tokenService;
        private readonly ILoginGoogleService _loginGoogleService;

        public AuthService(
            IUsuarioService usuarioService,
            IAdminService adminService,
            ITokenService tokenService,
            ILoginGoogleService loginGoogleService)
        {
            _usuarioService = usuarioService;
            _adminService = adminService;
            _tokenService = tokenService;
            _loginGoogleService = loginGoogleService;
        }

        // ============================================
        // LOGIN DE USUARIO CON GOOGLE
        // ============================================
        public async Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginGoogleAsync(
            string googleSub,
            string email,
            string nombre,
            string? avatarUrl,
            string? refreshToken)
        {
            // Crear o actualizar el login de Google (y crear usuario si no existe)
            var loginGoogle = await _loginGoogleService.CreateOrUpdateGoogleLoginAsync(
                googleSub,
                email,
                nombre,
                avatarUrl,
                refreshToken
            );

            var usuario = loginGoogle.Usuarios;

            // Generar tokens JWT
            var accessToken = await _tokenService.GenerateAccessTokenAsync(usuario.id);
            var refreshTokenJwt = await _tokenService.GenerateRefreshTokenAsync();

            // Guardar refresh token
            await _tokenService.SaveTokenAsync(usuario.id, accessToken, refreshTokenJwt, 3600);

            return (usuario, accessToken, refreshTokenJwt);
        }

        // ============================================
        // LOGIN DE ADMIN
        // ============================================
        public async Task<(Admin Admin, string AccessToken, string RefreshToken)?> AdminLoginAsync(
            string usuario,
            string password)
        {
            // Autenticar admin con usuario + password
            var admin = await _adminService.AuthenticateAsync(usuario, password);
            if (admin == null)
                return null;

            // Generar tokens
            var accessToken = await _tokenService.GenerateAccessTokenAsync(admin.id);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            // Guardar refresh token
            await _tokenService.SaveTokenAsync(admin.id, accessToken, refreshToken, 3600);

            return (admin, accessToken, refreshToken);
        }

        // ============================================
        // TOKENS
        // ============================================
        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            return await _tokenService.RefreshJwtTokenAsync(refreshToken);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            var token = await _tokenService.GetByRefreshTokenAsync(refreshToken);
            if (token == null)
                return false;

            return await _tokenService.RevokeTokenAsync(token.usuarioId);
        }

        public async Task<bool> LogoutAllAsync(int usuarioId)
        {
            return await _tokenService.RevokeTokenAsync(usuarioId);
        }
    }
}