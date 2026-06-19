using System;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class LoginGoogleRepository : GenericRepository<LoginGoogle>, ILoginGoogleRepository
    {
        private readonly AppDbContext _context;

        public LoginGoogleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LoginGoogle?> GetByGoogleSubAsync(string googleSub)
        {
            return await _context.LoginGoogle
                .FirstOrDefaultAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task<LoginGoogle?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.LoginGoogle
                .FirstOrDefaultAsync(lg => lg.usuarioId == usuarioId && lg.deletedAt == null);
        }

        public async Task<LoginGoogle?> GetWithUsuarioAsync(int id)
        {
            return await _context.LoginGoogle
                .Include(lg => lg.Usuarios)
                .FirstOrDefaultAsync(lg => lg.id == id && lg.deletedAt == null);
        }

        public async Task<LoginGoogle?> GetByGoogleSubWithUsuarioAsync(string googleSub)
        {
            return await _context.LoginGoogle
                .Include(lg => lg.Usuarios)
                .FirstOrDefaultAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task<bool> ExistsByGoogleSubAsync(string googleSub)
        {
            return await _context.LoginGoogle
                .AnyAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
        {
            return await _context.LoginGoogle
                .AnyAsync(lg => lg.usuarioId == usuarioId && lg.deletedAt == null);
        }

        public async Task UpdateLastLoginAsync(int id)
        {
            var loginGoogle = await _context.LoginGoogle.FindAsync(id);
            if (loginGoogle != null && loginGoogle.deletedAt == null)
            {
                loginGoogle.ultimoLogin = DateTime.Now;
                loginGoogle.updatedAt = DateTime.Now;
                await SaveChangesAsync();
            }
        }

        public async Task<bool> SoftDeleteByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var loginGoogle = await GetByUsuarioIdAsync(usuarioId);
                if (loginGoogle == null)
                    return false;

                loginGoogle.deletedAt = DateTime.Now;
                loginGoogle.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<LoginGoogle?> GetActiveByUsuarioIdAsync(int usuarioId)
        {
            return await _context.LoginGoogle
                .FirstOrDefaultAsync(lg => lg.usuarioId == usuarioId && lg.deletedAt == null);
        }
    }
}