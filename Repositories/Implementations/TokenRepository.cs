using System;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class TokenRepository : GenericRepository<SantanderToken>, ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SantanderToken?> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.SantanderTokens
                .FirstOrDefaultAsync(t => t.usuarioId == usuarioId);
        }

        public async Task<SantanderToken?> GetValidTokenAsync(int usuarioId)
        {
            var token = await GetByUsuarioAsync(usuarioId);
            if (token == null)
                return null;

            // Verificar si el token expiró (asumiendo que expiresIn está en segundos)
            var timeElapsed = DateTime.Now - token.createdAt;
            if (timeElapsed.TotalSeconds >= token.expiresIn)
                return null;

            return token;
        }

        public async Task<bool> TokenExistsAsync(int usuarioId)
        {
            return await _context.SantanderTokens
                .AnyAsync(t => t.usuarioId == usuarioId);
        }

        public async Task<bool> UpdateTokenAsync(int usuarioId, string accessToken, string refreshToken, int expiresIn)
        {
            try
            {
                var token = await GetByUsuarioAsync(usuarioId);
                if (token == null)
                {
                    // Crear nuevo token
                    token = new SantanderToken
                    {
                        usuarioId = usuarioId,
                        accessToken = accessToken,
                        refreshToken = refreshToken,
                        expiresIn = expiresIn,
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now
                    };
                    await AddAsync(token);
                }
                else
                {
                    // Actualizar token existente
                    token.accessToken = accessToken;
                    token.refreshToken = refreshToken;
                    token.expiresIn = expiresIn;
                    token.updatedAt = DateTime.Now;
                    Update(token);
                }

                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RevokeTokenAsync(int usuarioId)
        {
            try
            {
                var token = await GetByUsuarioAsync(usuarioId);
                if (token == null)
                    return false;

                Remove(token);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsTokenExpiredAsync(int usuarioId)
        {
            var token = await GetByUsuarioAsync(usuarioId);
            if (token == null)
                return true;

            var timeElapsed = DateTime.Now - token.createdAt;
            return timeElapsed.TotalSeconds >= token.expiresIn;
        }

        public async Task<SantanderToken?> GetTokenWithUsuarioAsync(int id)
        {
            return await _context.SantanderTokens
                .Include(t => t.Usuarios)
                .FirstOrDefaultAsync(t => t.id == id);
        }

        public async Task<SantanderToken?> GetByAccessTokenAsync(string accessToken)
        {
            return await _context.SantanderTokens
                .FirstOrDefaultAsync(t => t.accessToken == accessToken);
        }

        public async Task<SantanderToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.SantanderTokens
                .FirstOrDefaultAsync(t => t.refreshToken == refreshToken);
        }
    }
}