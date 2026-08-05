using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class PushTokenService : IPushTokenService
    {
        private readonly IPushTokenRepository _pushTokenRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAdminService _adminService; // Asumiendo que existe

        public PushTokenService(
            IPushTokenRepository pushTokenRepository,
            IUsuarioRepository usuarioRepository)
        {
            _pushTokenRepository = pushTokenRepository;
            _usuarioRepository = usuarioRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<IEnumerable<PushToken>> GetByUsuarioAsync(int usuarioId)
        {
            return await _pushTokenRepository.GetByUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<PushToken>> GetByAdminAsync(int adminId)
        {
            return await _pushTokenRepository.GetByAdminAsync(adminId);
        }

        public async Task<PushToken?> GetByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return await _pushTokenRepository.GetByTokenAsync(token);
        }

        public async Task<IEnumerable<PushToken>> GetByDispositivoAsync(string dispositivo)
        {
            if (string.IsNullOrWhiteSpace(dispositivo))
                return Enumerable.Empty<PushToken>();

            return await _pushTokenRepository.GetByDispositivoAsync(dispositivo);
        }

        public async Task<IEnumerable<PushToken>> GetActiveUserTokensAsync()
        {
            return await _pushTokenRepository.GetActiveUserTokensAsync();
        }

        public async Task<IEnumerable<PushToken>> GetActiveAdminTokensAsync()
        {
            return await _pushTokenRepository.GetActiveAdminTokensAsync();
        }

        public async Task<bool> TokenExistsAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            return await _pushTokenRepository.TokenExistsAsync(token);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<bool> RegisterUserTokenAsync(int usuarioId, string token, string dispositivo)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("El token no puede estar vacío", nameof(token));

            if (string.IsNullOrWhiteSpace(dispositivo))
                throw new ArgumentException("El dispositivo no puede estar vacío", nameof(dispositivo));

            // Verificar que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || usuario.deletedAt != null)
                throw new InvalidOperationException("El usuario no existe o está desactivado");

            return await _pushTokenRepository.RegisterUserTokenAsync(usuarioId, token, dispositivo);
        }

        public async Task<bool> RegisterAdminTokenAsync(int adminId, string token, string dispositivo)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("El token no puede estar vacío", nameof(token));

            if (string.IsNullOrWhiteSpace(dispositivo))
                throw new ArgumentException("El dispositivo no puede estar vacío", nameof(dispositivo));

            // Verificar que el admin existe
            // var admin = await _adminService.GetByIdAsync(adminId);
            // if (admin == null)
            //     throw new InvalidOperationException("El admin no existe");

            return await _pushTokenRepository.RegisterAdminTokenAsync(adminId, token, dispositivo);
        }

        public async Task<bool> RemoveTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            return await _pushTokenRepository.RemoveTokenAsync(token);
        }

        public async Task<bool> RemoveAllUserTokensAsync(int usuarioId)
        {
            return await _pushTokenRepository.RemoveAllUserTokensAsync(usuarioId);
        }

        public async Task<bool> RemoveAllAdminTokensAsync(int adminId)
        {
            return await _pushTokenRepository.RemoveAllAdminTokensAsync(adminId);
        }

        public async Task<IEnumerable<string>> GetUserTokensForNotificationAsync(int? usuarioId = null)
        {
            return await _pushTokenRepository.GetUserTokensForNotificationAsync(usuarioId);
        }

        public async Task<IEnumerable<string>> GetAdminTokensForNotificationAsync(int? adminId = null)
        {
            return await _pushTokenRepository.GetAdminTokensForNotificationAsync(adminId);
        }

        // ============================================
        // ENVÍO DE NOTIFICACIONES (Mock)
        // ============================================

        public async Task<bool> SendPushToUserAsync(int usuarioId, string titulo, string mensaje, object? data = null)
        {
            try
            {
                var tokens = await GetUserTokensForNotificationAsync(usuarioId);
                if (!tokens.Any())
                    return false;

                // Aquí se integraría con Firebase Cloud Messaging (FCM) o similar
                // Ejemplo:
                // foreach (var token in tokens)
                // {
                //     await _firebaseService.SendNotificationAsync(token, titulo, mensaje, data);
                // }

                // Simulación de envío exitoso
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> SendPushToAllUsersAsync(string titulo, string mensaje, object? data = null)
        {
            try
            {
                var tokens = await GetUserTokensForNotificationAsync();
                if (!tokens.Any())
                    return 0;

                // Aquí se integraría con Firebase Cloud Messaging (FCM) o similar
                // foreach (var token in tokens)
                // {
                //     await _firebaseService.SendNotificationAsync(token, titulo, mensaje, data);
                // }

                return tokens.Count();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> SendPushToAllAdminsAsync(string titulo, string mensaje, object? data = null)
        {
            try
            {
                var tokens = await GetAdminTokensForNotificationAsync();
                if (!tokens.Any())
                    return 0;

                // Aquí se integraría con Firebase Cloud Messaging (FCM) o similar
                // foreach (var token in tokens)
                // {
                //     await _firebaseService.SendNotificationAsync(token, titulo, mensaje, data);
                // }

                return tokens.Count();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> SendPushToUserListAsync(IEnumerable<int> usuarioIds, string titulo, string mensaje, object? data = null)
        {
            try
            {
                if (usuarioIds == null || !usuarioIds.Any())
                    return 0;

                var allTokens = new List<string>();
                foreach (var userId in usuarioIds)
                {
                    var tokens = await GetUserTokensForNotificationAsync(userId);
                    allTokens.AddRange(tokens);
                }

                if (!allTokens.Any())
                    return 0;

                // Aquí se integraría con Firebase Cloud Messaging (FCM) o similar
                // foreach (var token in allTokens.Distinct())
                // {
                //     await _firebaseService.SendNotificationAsync(token, titulo, mensaje, data);
                // }

                return allTokens.Distinct().Count();
            }
            catch
            {
                return 0;
            }
        }
    }
}