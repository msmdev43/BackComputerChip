using System;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class LoginGoogleService : ILoginGoogleService
    {
        private readonly ILoginGoogleRepository _loginGoogleRepository;
        private readonly IUsuarioService _usuarioService;

        public LoginGoogleService(
            ILoginGoogleRepository loginGoogleRepository,
            IUsuarioService usuarioService)
        {
            _loginGoogleRepository = loginGoogleRepository;
            _usuarioService = usuarioService;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<LoginGoogle?> GetByGoogleSubAsync(string googleSub)
        {
            return await _loginGoogleRepository.GetByGoogleSubAsync(googleSub);
        }

        public async Task<LoginGoogle?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _loginGoogleRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<LoginGoogle?> GetByGoogleSubWithUsuarioAsync(string googleSub)
        {
            return await _loginGoogleRepository.GetByGoogleSubWithUsuarioAsync(googleSub);
        }

        public async Task<bool> ExistsByGoogleSubAsync(string googleSub)
        {
            return await _loginGoogleRepository.ExistsByGoogleSubAsync(googleSub);
        }

        public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
        {
            return await _loginGoogleRepository.ExistsByUsuarioIdAsync(usuarioId);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<LoginGoogle> CreateOrUpdateGoogleLoginAsync(
            string googleSub,
            string email,
            string nombre,
            string avatarUrl,
            string? refreshToken)
        {
            // Buscar por Google Sub
            var loginGoogle = await _loginGoogleRepository.GetByGoogleSubAsync(googleSub);

            if (loginGoogle != null)
            {
                // Actualizar datos
                loginGoogle.email = email;
                loginGoogle.nombre = nombre;
                loginGoogle.avatarUrl = avatarUrl;
                loginGoogle.ultimoLogin = DateTime.Now;
                loginGoogle.updatedAt = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(refreshToken))
                {
                    loginGoogle.refreshToken = refreshToken;
                }

                _loginGoogleRepository.Update(loginGoogle);
                await _loginGoogleRepository.SaveChangesAsync();
                return loginGoogle;
            }

            // Crear nuevo usuario
            var usuario = new Usuarios
            {
                nombreCompleto = nombre,
                email = email,
                emailVerify = true, // Google ya verificó el email
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            await _usuarioService.CreateUserAsync(usuario, null);

            // Crear login Google
            loginGoogle = new LoginGoogle
            {
                usuarioId = usuario.id,
                googleSub = googleSub,
                email = email,
                emailVerificado = true,
                nombre = nombre,
                avatarUrl = avatarUrl,
                refreshToken = refreshToken,
                ultimoLogin = DateTime.Now,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            await _loginGoogleRepository.AddAsync(loginGoogle);
            await _loginGoogleRepository.SaveChangesAsync();

            return loginGoogle;
        }

        public async Task<bool> UpdateLastLoginAsync(int id)
        {
            try
            {
                await _loginGoogleRepository.UpdateLastLoginAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync(int id, string refreshToken)
        {
            try
            {
                var loginGoogle = await _loginGoogleRepository.GetByIdAsync(id);
                if (loginGoogle == null)
                    return false;

                loginGoogle.refreshToken = refreshToken;
                loginGoogle.updatedAt = DateTime.Now;

                _loginGoogleRepository.Update(loginGoogle);
                return await _loginGoogleRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteByUsuarioIdAsync(int usuarioId)
        {
            return await _loginGoogleRepository.SoftDeleteByUsuarioIdAsync(usuarioId);
        }
    }
}