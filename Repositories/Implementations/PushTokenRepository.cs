using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class PushTokenRepository : GenericRepository<PushToken>, IPushTokenRepository
    {
        private readonly AppDbContext _context;

        public PushTokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PushToken>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.PushTokens
                .Include(pt => pt.Usuarios)
                .Where(pt => pt.usuarioId == usuarioId)
                .OrderByDescending(pt => pt.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PushToken>> GetByAdminAsync(int adminId)
        {
            return await _context.PushTokens
                .Include(pt => pt.Admin)
                .Where(pt => pt.adminId == adminId)
                .OrderByDescending(pt => pt.createdAt)
                .ToListAsync();
        }

        public async Task<PushToken?> GetByTokenAsync(string token)
        {
            return await _context.PushTokens
                .Include(pt => pt.Usuarios)
                .Include(pt => pt.Admin)
                .FirstOrDefaultAsync(pt => pt.token == token);
        }

        public async Task<IEnumerable<PushToken>> GetByDispositivoAsync(string dispositivo)
        {
            return await _context.PushTokens
                .Where(pt => pt.dispositivo == dispositivo)
                .OrderByDescending(pt => pt.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PushToken>> GetActiveUserTokensAsync()
        {
            return await _context.PushTokens
                .Include(pt => pt.Usuarios)
                .Where(pt => pt.usuarioId != null && pt.Usuarios != null && pt.Usuarios.deletedAt == null)
                .OrderByDescending(pt => pt.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PushToken>> GetActiveAdminTokensAsync()
        {
            return await _context.PushTokens
                .Include(pt => pt.Admin)
                .Where(pt => pt.adminId != null)
                .OrderByDescending(pt => pt.createdAt)
                .ToListAsync();
        }

        public async Task<bool> RegisterUserTokenAsync(int usuarioId, string token, string dispositivo)
        {
            try
            {
                // Verificar si el token ya existe
                var existing = await GetByTokenAsync(token);
                if (existing != null)
                {
                    // Si el token ya existe pero para otro usuario, actualizar
                    if (existing.usuarioId != usuarioId)
                    {
                        existing.usuarioId = usuarioId;
                        existing.adminId = null;
                        existing.dispositivo = dispositivo;
                        existing.updatedAt = DateTime.Now;
                        Update(existing);
                        return await SaveChangesAsync();
                    }
                    // Si ya existe para este usuario, no hacer nada
                    return true;
                }

                // Crear nuevo token
                var pushToken = new PushToken
                {
                    usuarioId = usuarioId,
                    adminId = null,
                    token = token,
                    dispositivo = dispositivo,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };

                await AddAsync(pushToken);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegisterAdminTokenAsync(int adminId, string token, string dispositivo)
        {
            try
            {
                var existing = await GetByTokenAsync(token);
                if (existing != null)
                {
                    if (existing.adminId != adminId)
                    {
                        existing.adminId = adminId;
                        existing.usuarioId = null;
                        existing.dispositivo = dispositivo;
                        existing.updatedAt = DateTime.Now;
                        Update(existing);
                        return await SaveChangesAsync();
                    }
                    return true;
                }

                var pushToken = new PushToken
                {
                    usuarioId = null,
                    adminId = adminId,
                    token = token,
                    dispositivo = dispositivo,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };

                await AddAsync(pushToken);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveTokenAsync(string token)
        {
            try
            {
                var pushToken = await GetByTokenAsync(token);
                if (pushToken == null)
                    return false;

                Remove(pushToken);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAllUserTokensAsync(int usuarioId)
        {
            try
            {
                var tokens = await _context.PushTokens
                    .Where(pt => pt.usuarioId == usuarioId)
                    .ToListAsync();

                if (tokens.Any())
                {
                    _context.PushTokens.RemoveRange(tokens);
                    return await SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAllAdminTokensAsync(int adminId)
        {
            try
            {
                var tokens = await _context.PushTokens
                    .Where(pt => pt.adminId == adminId)
                    .ToListAsync();

                if (tokens.Any())
                {
                    _context.PushTokens.RemoveRange(tokens);
                    return await SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TokenExistsAsync(string token)
        {
            return await _context.PushTokens
                .AnyAsync(pt => pt.token == token);
        }

        public async Task<IEnumerable<string>> GetUserTokensForNotificationAsync(int? usuarioId = null)
        {
            var query = _context.PushTokens
                .Include(pt => pt.Usuarios)
                .Where(pt => pt.usuarioId != null && pt.Usuarios != null && pt.Usuarios.deletedAt == null);

            if (usuarioId.HasValue)
            {
                query = query.Where(pt => pt.usuarioId == usuarioId.Value);
            }

            return await query
                .Select(pt => pt.token)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAdminTokensForNotificationAsync(int? adminId = null)
        {
            var query = _context.PushTokens
                .Where(pt => pt.adminId != null);

            if (adminId.HasValue)
            {
                query = query.Where(pt => pt.adminId == adminId.Value);
            }

            return await query
                .Select(pt => pt.token)
                .Distinct()
                .ToListAsync();
        }
    }
}