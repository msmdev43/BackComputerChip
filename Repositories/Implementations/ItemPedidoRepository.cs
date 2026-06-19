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
    public class ItemPedidoRepository : GenericRepository<ItemPedido>, IItemPedidoRepository
    {
        private readonly AppDbContext _context;

        public ItemPedidoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoAsync(int pedidoId)
        {
            return await _context.ItemsPedido
                .Where(i => i.PedidosItemPedido.Any(pip => pip.pedidosId == pedidoId))
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemPedido>> GetByProductoAsync(int productoId)
        {
            return await _context.ItemsPedido
                .Where(i => i.ItemPedidoProductos.Any(ip => ip.productoId == productoId))
                .ToListAsync();
        }

        public async Task<ItemPedido?> GetWithFullDetailsAsync(int id)
        {
            return await _context.ItemsPedido
                .Include(i => i.ItemPedidoProductos)
                    .ThenInclude(ip => ip.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .Include(i => i.PedidosItemPedido)
                    .ThenInclude(pip => pip.Pedidos)
                        .ThenInclude(p => p.Usuarios)
                .FirstOrDefaultAsync(i => i.id == id);
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoWithProductosAsync(int pedidoId)
        {
            return await _context.ItemsPedido
                .Include(i => i.ItemPedidoProductos)
                    .ThenInclude(ip => ip.Productos)
                .Where(i => i.PedidosItemPedido.Any(pip => pip.pedidosId == pedidoId))
                .ToListAsync();
        }

        public async Task<decimal> GetSubtotalByPedidoAsync(int pedidoId)
        {
            return await _context.ItemsPedido
                .Where(i => i.PedidosItemPedido.Any(pip => pip.pedidosId == pedidoId))
                .SumAsync(i => i.subtotal);
        }

        public async Task<int> GetTotalItemsByPedidoAsync(int pedidoId)
        {
            return await _context.ItemsPedido
                .Where(i => i.PedidosItemPedido.Any(pip => pip.pedidosId == pedidoId))
                .SumAsync(i => i.cantidad);
        }

        public async Task<IEnumerable<ItemPedido>> GetRecentItemsAsync(int days)
        {
            var since = DateTime.Now.AddDays(-days);
            return await _context.ItemsPedido
                .Include(i => i.ItemPedidoProductos)
                    .ThenInclude(ip => ip.Productos)
                .Where(i => i.PedidosItemPedido.Any(pip => pip.Pedidos.createdAt >= since))
                .OrderByDescending(i => i.id)
                .ToListAsync();
        }
    }
}