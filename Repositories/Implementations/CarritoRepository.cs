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
    public class CarritoRepository : GenericRepository<Carrito>, ICarritoRepository
    {
        private readonly AppDbContext _context;

        public CarritoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Carrito?> GetActiveCarritoByUsuarioAsync(int usuarioId)
        {
            return await _context.Carritos
                .FirstOrDefaultAsync(c => c.usuarioId == usuarioId
                                         && c.estado == "activo");
        }

        public async Task<Carrito?> GetCarritoWithProductsAsync(int carritoId)
        {
            return await _context.Carritos
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Productos)
                        .ThenInclude(p => p.ProductosImagenes)
                            .ThenInclude(pi => pi.Imagenes)
                .FirstOrDefaultAsync(c => c.id == carritoId);
        }

        public async Task<Carrito?> GetActiveCarritoWithProductsAsync(int usuarioId)
        {
            var carrito = await GetActiveCarritoByUsuarioAsync(usuarioId);
            if (carrito == null)
                return null;

            return await GetCarritoWithProductsAsync(carrito.id);
        }

        public async Task<bool> AddProductToCarritoAsync(int carritoId, int productoId, int cantidad, decimal precioUnitario)
        {
            try
            {
                var carrito = await _context.Carritos
                    .Include(c => c.CarritoProductos)
                    .FirstOrDefaultAsync(c => c.id == carritoId);

                if (carrito == null)
                    return false;

                var existingItem = carrito.CarritoProductos
                    .FirstOrDefault(cp => cp.productoId == productoId);

                if (existingItem != null)
                {
                    existingItem.cantidad += cantidad;
                    existingItem.precioUnitario = precioUnitario;
                    existingItem.updatedAt = DateTime.Now;
                }
                else
                {
                    carrito.CarritoProductos.Add(new CarritoProductos
                    {
                        carritoId = carritoId,
                        productoId = productoId,
                        cantidad = cantidad,
                        precioUnitario = precioUnitario,
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now
                    });
                }

                carrito.updatedAt = DateTime.Now;
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveProductFromCarritoAsync(int carritoId, int productoId)
        {
            try
            {
                var item = await _context.CarritoProductos
                    .FirstOrDefaultAsync(cp => cp.carritoId == carritoId
                                             && cp.productoId == productoId);

                if (item == null)
                    return false;

                _context.CarritoProductos.Remove(item);
                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductQuantityAsync(int carritoId, int productoId, int cantidad)
        {
            try
            {
                var item = await _context.CarritoProductos
                    .FirstOrDefaultAsync(cp => cp.carritoId == carritoId
                                             && cp.productoId == productoId);

                if (item == null)
                    return false;

                if (cantidad <= 0)
                {
                    _context.CarritoProductos.Remove(item);
                }
                else
                {
                    item.cantidad = cantidad;
                    item.updatedAt = DateTime.Now;
                }

                return await SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ClearCarritoAsync(int carritoId)
        {
            try
            {
                var items = await _context.CarritoProductos
                    .Where(cp => cp.carritoId == carritoId)
                    .ToListAsync();

                if (items.Any())
                {
                    _context.CarritoProductos.RemoveRange(items);
                    return await SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> GetCarritoTotalAsync(int carritoId)
        {
            var items = await _context.CarritoProductos
                .Where(cp => cp.carritoId == carritoId)
                .ToListAsync();

            return items.Sum(item => item.cantidad * item.precioUnitario);
        }

        public async Task<int> GetCarritoItemCountAsync(int carritoId)
        {
            return await _context.CarritoProductos
                .Where(cp => cp.carritoId == carritoId)
                .SumAsync(cp => cp.cantidad);
        }

        public async Task<int> GetCarritoDistinctProductsCountAsync(int carritoId)
        {
            return await _context.CarritoProductos
                .Where(cp => cp.carritoId == carritoId)
                .CountAsync();
        }

        public async Task<Carrito?> ConvertCarritoToPedidoAsync(int carritoId)
        {
            try
            {
                var carrito = await GetCarritoWithProductsAsync(carritoId);
                if (carrito == null || !carrito.CarritoProductos.Any())
                    return null;

                carrito.estado = "convertido";
                carrito.updatedAt = DateTime.Now;

                await SaveChangesAsync();
                return carrito;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> HasActiveCarritoAsync(int usuarioId)
        {
            return await _context.Carritos
                .AnyAsync(c => c.usuarioId == usuarioId && c.estado == "activo");
        }

        public async Task<Carrito?> CreateCarritoForUsuarioAsync(int usuarioId)
        {
            try
            {
                var existingCarrito = await GetActiveCarritoByUsuarioAsync(usuarioId);
                if (existingCarrito != null)
                    return existingCarrito;

                var carrito = new Carrito
                {
                    usuarioId = usuarioId,
                    estado = "activo",
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };

                await AddAsync(carrito);
                await SaveChangesAsync();
                return carrito;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RemoveAllInactiveCarritosAsync(int usuarioId)
        {
            try
            {
                var carritos = await _context.Carritos
                    .Where(c => c.usuarioId == usuarioId && c.estado != "activo")
                    .ToListAsync();

                if (carritos.Any())
                {
                    _context.Carritos.RemoveRange(carritos);
                    return await SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}