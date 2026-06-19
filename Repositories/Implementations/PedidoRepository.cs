using computerChip.Data;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace computerChip.Repositories.Implementations
{
    public class PedidoRepository : GenericRepository<Pedidos>, IPedidoRepository
    {
        private readonly AppDbContext _context;
        private readonly ICarritoRepository _carritoRepository;

        public PedidoRepository(AppDbContext context, ICarritoRepository carritoRepository)
            : base(context)
        {
            _context = context;
            _carritoRepository = carritoRepository;
        }

        public async Task<IEnumerable<Pedidos>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.Pedidos
                .Include(p => p.Items)
                    .ThenInclude(i => i.ItemPedidoProductos)
                        .ThenInclude(ip => ip.Productos)
                            .ThenInclude(pr => pr.ProductosImagenes)
                .Include(p => p.MetodoPago)
                .Include(p => p.ZonaEnvio)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedidos>> GetByEstadoAsync(string estado)
        {
            return await _context.Pedidos
                .Include(p => p.Usuarios)
                .Include(p => p.MetodoPago)
                .Where(p => p.estado == estado)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<Pedidos?> GetWithFullDetailsAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Usuarios)
                .Include(p => p.MetodoPago)
                .Include(p => p.ZonaEnvio)
                .Include(p => p.Ofertas)
                .Include(p => p.Items)
                    .ThenInclude(i => i.ItemPedidoProductos)
                        .ThenInclude(ip => ip.Productos)
                            .ThenInclude(pr => pr.ProductosImagenes)
                                .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(p => p.id == pedidoId);
        }

        public async Task<IEnumerable<Pedidos>> GetPendingPedidosAsync()
        {
            return await GetByEstadoAsync("pendiente");
        }

        public async Task<IEnumerable<Pedidos>> GetRecentPedidosAsync(int days)
        {
            var since = DateTime.Now.AddDays(-days);
            return await _context.Pedidos
                .Include(p => p.Usuarios)
                .Where(p => p.createdAt >= since)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalVentasAsync()
        {
            return await _context.Pedidos
                .Where(p => p.estado != "cancelado")
                .SumAsync(p => p.total);
        }

        public async Task<decimal> GetTotalVentasByPeriodoAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Pedidos
                .Where(p => p.createdAt >= desde && p.createdAt <= hasta && p.estado != "cancelado")
                .SumAsync(p => p.total);
        }

        public async Task<int> GetPedidosCountByEstadoAsync(string estado)
        {
            return await _context.Pedidos
                .CountAsync(p => p.estado == estado);
        }

        public async Task<bool> UpdateEstadoAsync(int pedidoId, string nuevoEstado)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(pedidoId);
                if (pedido == null)
                    return false;

                pedido.estado = nuevoEstado;
                pedido.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<Pedidos?> CreatePedidoFromCarritoAsync(int carritoId)
        {
            try
            {
                var carrito = await _carritoRepository.GetCarritoWithProductsAsync(carritoId);
                if (carrito == null || !carrito.CarritoProductos.Any())
                    return null;

                var total = await _carritoRepository.GetCarritoTotalAsync(carritoId);

                var items = carrito.CarritoProductos.Select(cp => new ItemPedido
                {
                    cantidad = cp.cantidad,
                    subtotal = cp.cantidad * cp.precioUnitario,
                    ItemPedidoProductos = new List<ItemPedidoProductos>
                    {
                        new ItemPedidoProductos
                        {
                            productoId = cp.productoId
                        }
                    }
                }).ToList();

                var pedido = new Pedidos
                {
                    UsuarioId = carrito.usuarioId,
                    MetodoPagoId = 1,
                    ZonaEnvioId = 1,
                    estado = "pendiente",
                    total = total,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    Items = items
                };

                await AddAsync(pedido);
                await SaveChangesAsync();

                await _carritoRepository.ConvertCarritoToPedidoAsync(carritoId);

                return pedido;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Pedidos>> GetAllWithDetailsAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Usuarios)
                .Include(p => p.MetodoPago)
                .Include(p => p.ZonaEnvio)
                .Include(p => p.Items)
                    .ThenInclude(i => i.ItemPedidoProductos)
                        .ThenInclude(ip => ip.Productos)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedidos>> GetPedidosByDateRangeAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Pedidos
                .Include(p => p.Usuarios)
                .Include(p => p.MetodoPago)
                .Where(p => p.createdAt >= desde && p.createdAt <= hasta)
                .OrderByDescending(p => p.createdAt)
                .ToListAsync();
        }

        public async Task<decimal> GetPromedioVentaAsync()
        {
            return await _context.Pedidos
                .Where(p => p.estado != "cancelado")
                .AverageAsync(p => p.total);
        }

        public async Task<decimal> GetMaxVentaAsync()
        {
            return await _context.Pedidos
                .Where(p => p.estado != "cancelado")
                .MaxAsync(p => p.total);
        }

        public async Task<int> GetTotalPedidosAsync()
        {
            return await _context.Pedidos
                .CountAsync(p => p.estado != "cancelado");
        }

        public async Task<bool> CancelPedidoAsync(int pedidoId)
        {
            return await UpdateEstadoAsync(pedidoId, "cancelado");
        }

        public async Task<bool> ConfirmPedidoAsync(int pedidoId)
        {
            return await UpdateEstadoAsync(pedidoId, "confirmado");
        }
    }
}