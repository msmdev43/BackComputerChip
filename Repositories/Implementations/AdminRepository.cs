using System;
using System.Linq;
using System.Threading.Tasks;
using computerChip.Data;
using computerChip.Models;
using computerChip.Models.Enum;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Repositories.Implementations
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Admin?> GetByUsuarioAsync(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return null;

            return await _context.Admins
                .FirstOrDefaultAsync(a => a.usuario.ToLower() == usuario.ToLower());
        }

        public async Task<int> GetTotalPedidosAsync()
        {
            return await _context.Pedidos
                .CountAsync(p => p.estado != EstadoPedido.CANCELADO);
        }

        public async Task<int> GetPedidosHoyAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            return await _context.Pedidos
                .CountAsync(p => p.createdAt >= hoy && p.createdAt < manana);
        }

        public async Task<int> GetCantidadPedidos()
        {
            return await _context.Pedidos
                .Where(p => p.estado != EstadoPedido.CANCELADO)
                .SumAsync(p => p.Items.Sum(i => i.cantidad));
        }

        public async Task<int> GetTotalUsuariosAsync()
        {
            return await _context.Usuarios
                .CountAsync(u => u.deletedAt == null);
        }

        public async Task<int> GetTotalCategoriasAsync()
        {
            return await _context.Categorias
                .CountAsync(c => c.deletedAt == null);
        }

        public async Task<int> GetCantidadProductosPorCategoriaAsync()
        {
            return await _context.CategoriasProductos
                .GroupBy(cp => cp.categoriaId)
                .CountAsync();
        }
    }
}