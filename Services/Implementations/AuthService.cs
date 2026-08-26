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
        // REGISTRO Y LOGIN
        // ============================================

        public async Task<(Usuarios Usuario, string AccessToken, string RefreshToken)> RegisterAsync(
            string nombreCompleto,
            string email,
            string password,
            string? pais = null,
            string? provincia = null,
            string? ciudad = null,
            string? calle = null,
            string? numero = null,
            string? celular = null)
        {
            // Crear usuario
            var usuario = new Usuarios
            {
                nombreCompleto = nombreCompleto,
                email = email,
                pais = pais,
                provincia = provincia,
                ciudad = ciudad,
                calle = calle,
                numero = numero,
                celular = celular,
                emailVerify = false,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            var created = await _usuarioService.CreateUserAsync(usuario, password);

            // Generar tokens
            var accessToken = await _tokenService.GenerateAccessTokenAsync(created.id);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            // Guardar refresh token
            await _tokenService.SaveTokenAsync(created.id, accessToken, refreshToken, 3600);

            return (created, accessToken, refreshToken);
        }

        public async Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginAsync(string email, string password)
        {
            // Autenticar usuario
            var usuario = await _usuarioService.AuthenticateAsync(email, password);
            if (usuario == null)
                return null;

            // Generar tokens
            var accessToken = await _tokenService.GenerateAccessTokenAsync(usuario.id);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            // Guardar refresh token
            await _tokenService.SaveTokenAsync(usuario.id, accessToken, refreshToken, 3600);

            return (usuario, accessToken, refreshToken);
        }

        public async Task<(Usuarios Usuario, string AccessToken, string RefreshToken)?> LoginGoogleAsync(
            string googleSub,
            string email,
            string nombre,
            string? avatarUrl,
            string? refreshToken)
        {
            // Crear o actualizar login Google
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

        public async Task<(Admin Admin, string AccessToken, string RefreshToken)?> AdminLoginAsync(string usuario, string password)
        {
            // Autenticar admin
            var admin = await _adminService.AuthenticateAsync(usuario, password);
            if (admin == null)
                return null;

            // Generar tokens (usando el ID del admin)
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

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await _tokenService.ValidateAccessTokenAsync(token);
        }

        public async Task<int?> GetUsuarioIdFromTokenAsync(string token)
        {
            return await _tokenService.GetUsuarioIdFromTokenAsync(token);
        }

        // ============================================
        // VERIFICACIÓN DE EMAIL
        // ============================================

        public async Task<bool> VerifyEmailAsync(int usuarioId)
        {
            return await _usuarioService.VerifyEmailAsync(usuarioId);
        }

        public async Task<bool> SendVerificationEmailAsync(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(usuarioId);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                if (usuario.emailVerify == true)
                    return true;

                // Aquí se enviaría el email de verificación
                // await _emailService.SendVerificationEmailAsync(usuario.email, token);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}