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
    public class UsuarioRepository : GenericRepository<Usuarios>, IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuarios?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.email == email && u.deletedAt == null);
        }

        public async Task<Usuarios?> GetByEmailWithIncludesAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.LoginGoogle)
                .Include(u => u.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                .FirstOrDefaultAsync(u => u.email == email && u.deletedAt == null);
        }

        public async Task<Usuarios?> GetWithFullDetailsAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.LoginGoogle)
                .Include(u => u.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Productos)
                            .ThenInclude(p => p.ProductosImagenes)
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.Items)
                        .ThenInclude(i => i.ItemPedidoProductos)
                            .ThenInclude(ip => ip.Productos)
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.MetodoPago)
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.ZonaEnvio)
                .Include(u => u.PushTokens)
                .Include(u => u.SantanderTokens)
                .FirstOrDefaultAsync(u => u.id == id && u.deletedAt == null);
        }

        public async Task<IEnumerable<Usuarios>> GetAllActiveAsync()
        {
            return await _context.Usuarios
                .Where(u => u.deletedAt == null)
                .OrderByDescending(u => u.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuarios>> GetUsersWithGoogleLoginAsync()
        {
            return await _context.Usuarios
                .Include(u => u.LoginGoogle)
                .Where(u => u.LoginGoogle.Any() && u.deletedAt == null)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.email == email && u.deletedAt == null);
        }

        public async Task<Usuarios?> GetUserWithPedidosAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.MetodoPago)
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.ZonaEnvio)
                .Include(u => u.Pedidos)
                    .ThenInclude(p => p.Items)
                .FirstOrDefaultAsync(u => u.id == id && u.deletedAt == null);
        }

        public async Task<Usuarios?> GetUserWithCarritoAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Productos)
                            .ThenInclude(p => p.ProductosImagenes)
                                .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(u => u.id == id && u.deletedAt == null);
        }

        public async Task<Usuarios?> GetUserWithLoginGoogleAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.LoginGoogle)
                .FirstOrDefaultAsync(u => u.id == id && u.deletedAt == null);
        }

        public async Task<int> GetTotalActiveUsersAsync()
        {
            return await _context.Usuarios
                .CountAsync(u => u.deletedAt == null);
        }

        public async Task<IEnumerable<Usuarios>> GetRecentUsersAsync(int days)
        {
            var since = DateTime.Now.AddDays(-days);
            return await _context.Usuarios
                .Where(u => u.createdAt >= since && u.deletedAt == null)
                .OrderByDescending(u => u.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuarios>> SearchUsersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllActiveAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Usuarios
                .Where(u => (u.nombreCompleto != null && u.nombreCompleto.ToLower().Contains(searchTerm)) ||
                            (u.email != null && u.email.ToLower().Contains(searchTerm)) ||
                            (u.celular != null && u.celular.Contains(searchTerm)))
                .Where(u => u.deletedAt == null)
                .OrderBy(u => u.nombreCompleto)
                .ToListAsync();
        }

        public async Task<bool> UpdateUserDataAsync(int id, string nombreCompleto, string celular)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                usuario.nombreCompleto = nombreCompleto ?? usuario.nombreCompleto;
                usuario.celular = celular ?? usuario.celular;
                usuario.updatedAt = DateTime.Now;

                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null || usuario.deletedAt != null)
                    return false;

                usuario.deletedAt = DateTime.Now;
                usuario.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null || usuario.deletedAt == null)
                    return false;

                usuario.deletedAt = null;
                usuario.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}