using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICarritoRepository _carritoRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ICarritoRepository carritoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _carritoRepository = carritoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Usuarios?> GetByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuarios?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Usuarios>> GetAllActiveAsync()
        {
            return await _usuarioRepository.GetAllActiveAsync();
        }

        public async Task<IEnumerable<Usuarios>> GetUsersWithGoogleLoginAsync()
        {
            return await _usuarioRepository.GetUsersWithGoogleLoginAsync();
        }

        public async Task<Usuarios?> GetUserWithFullDetailsAsync(int id)
        {
            return await _usuarioRepository.GetWithFullDetailsAsync(id);
        }

        public async Task<IEnumerable<Usuarios>> SearchUsersAsync(string searchTerm)
        {
            return await _usuarioRepository.SearchUsersAsync(searchTerm);
        }

        public async Task<IEnumerable<Usuarios>> GetRecentUsersAsync(int days)
        {
            return await _usuarioRepository.GetRecentUsersAsync(days);
        }

        public async Task<int> GetTotalActiveUsersAsync()
        {
            return await _usuarioRepository.GetTotalActiveUsersAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _usuarioRepository.ExistsByEmailAsync(email);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Usuarios> CreateUserAsync(Usuarios usuario, string? password = null)
        {
            // Validar que el email no exista
            if (await _usuarioRepository.ExistsByEmailAsync(usuario.email))
                throw new InvalidOperationException("El email ya está registrado");

            // Hashear password si se proporciona
            if (!string.IsNullOrWhiteSpace(password))
            {
                usuario.password = BCrypt.Net.BCrypt.HashPassword(password);
            }

            usuario.createdAt = DateTime.Now;
            usuario.updatedAt = DateTime.Now;
            usuario.emailVerify = false;

            await _usuarioRepository.AddAsync(usuario);
            await _usuarioRepository.SaveChangesAsync();

            // Crear carrito para el nuevo usuario
            await _carritoRepository.CreateCarritoForUsuarioAsync(usuario.id);

            return usuario;
        }

        public async Task<bool> UpdateUserAsync(int id, string nombreCompleto, string celular)
        {
            return await _usuarioRepository.UpdateUserDataAsync(id, nombreCompleto, celular);
        }

        public async Task<bool> UpdateUserEmailAsync(int id, string email)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                // Verificar que el nuevo email no esté en uso
                if (await _usuarioRepository.ExistsByEmailAsync(email))
                    return false;

                usuario.email = email;
                usuario.updatedAt = DateTime.Now;
                usuario.emailVerify = false;

                _usuarioRepository.Update(usuario);
                return await _usuarioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                // Verificar que tenga password (no es usuario de Google)
                if (string.IsNullOrWhiteSpace(usuario.password))
                    return false;

                // Verificar password actual
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, usuario.password))
                    return false;

                // Hashear nueva password
                usuario.password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                usuario.updatedAt = DateTime.Now;

                _usuarioRepository.Update(usuario);
                return await _usuarioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            return await _usuarioRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RestoreUserAsync(int id)
        {
            return await _usuarioRepository.RestoreAsync(id);
        }

        public async Task<bool> DeleteUserPermanentlyAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                    return false;

                _usuarioRepository.Remove(usuario);
                return await _usuarioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        // ============================================
        // OPERACIONES DE AUTENTICACIÓN
        // ============================================

        public async Task<Usuarios?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || usuario.deletedAt != null)
                return null;

            // Verificar que tenga password (no es usuario de Google)
            if (string.IsNullOrWhiteSpace(usuario.password))
                return null;

            // Verificar password
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.password))
                return null;

            return usuario;
        }

        public async Task<bool> VerifyEmailAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                usuario.emailVerify = true;
                usuario.updatedAt = DateTime.Now;

                _usuarioRepository.Update(usuario);
                return await _usuarioRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            // Implementar envío de email con token de reseteo
            // Este método sería llamado desde un servicio de email
            throw new NotImplementedException("Implementar envío de email para reset password");
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            // Implementar reseteo de password con token
            throw new NotImplementedException("Implementar reseteo de password con token");
        }
    }
}