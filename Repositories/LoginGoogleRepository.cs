using computerChip.Data;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories
{
    public class LoginGoogleRepository : GenericRepository<LoginGoogle>, ILoginGoogleRepository
    {
        public LoginGoogleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<LoginGoogle> GetByGoogleSubAsync(string googleSub)
        {
            return await _dbSet
                .FirstOrDefaultAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task<LoginGoogle> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(lg => lg.usuarioId == usuarioId && lg.deletedAt == null);
        }

        public async Task<LoginGoogle> GetWithUsuarioAsync(int id)
        {
            return await _dbSet
                .Include(lg => lg.Usuarios)
                .FirstOrDefaultAsync(lg => lg.id == id && lg.deletedAt == null);
        }

        public async Task<LoginGoogle> GetByGoogleSubWithUsuarioAsync(string googleSub)
        {
            return await _dbSet
                .Include(lg => lg.Usuarios)
                .FirstOrDefaultAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task<bool> ExistsByGoogleSubAsync(string googleSub)
        {
            return await _dbSet
                .AnyAsync(lg => lg.googleSub == googleSub && lg.deletedAt == null);
        }

        public async Task UpdateLastLoginAsync(int id)
        {
            var loginGoogle = await _dbSet.FindAsync(id);
            if (loginGoogle != null)
            {
                loginGoogle.ultimoLogin = DateTime.Now;
                loginGoogle.updatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
