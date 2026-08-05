using System;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;
        private readonly IProductoRepository _productoRepository;

        public CarritoService(
            ICarritoRepository carritoRepository,
            IProductoRepository productoRepository)
        {
            _carritoRepository = carritoRepository;
            _productoRepository = productoRepository;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<Carrito?> GetActiveCarritoByUsuarioAsync(int usuarioId)
        {
            return await _carritoRepository.GetActiveCarritoByUsuarioAsync(usuarioId);
        }

        public async Task<Carrito?> GetCarritoWithProductsAsync(int carritoId)
        {
            return await _carritoRepository.GetCarritoWithProductsAsync(carritoId);
        }

        public async Task<Carrito?> GetActiveCarritoWithProductsAsync(int usuarioId)
        {
            return await _carritoRepository.GetActiveCarritoWithProductsAsync(usuarioId);
        }

        public async Task<decimal> GetCarritoTotalAsync(int carritoId)
        {
            return await _carritoRepository.GetCarritoTotalAsync(carritoId);
        }

        public async Task<int> GetCarritoItemCountAsync(int carritoId)
        {
            return await _carritoRepository.GetCarritoItemCountAsync(carritoId);
        }

        public async Task<int> GetCarritoDistinctProductsCountAsync(int carritoId)
        {
            return await _carritoRepository.GetCarritoDistinctProductsCountAsync(carritoId);
        }

        public async Task<bool> HasActiveCarritoAsync(int usuarioId)
        {
            return await _carritoRepository.HasActiveCarritoAsync(usuarioId);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<Carrito> GetOrCreateCarritoForUsuarioAsync(int usuarioId)
        {
            var carrito = await _carritoRepository.GetActiveCarritoByUsuarioAsync(usuarioId);

            if (carrito == null)
            {
                carrito = await _carritoRepository.CreateCarritoForUsuarioAsync(usuarioId);
            }

            return carrito;
        }

        public async Task<bool> AddProductToCarritoAsync(int usuarioId, int productoId, int cantidad, decimal precioUnitario)
        {
            // Verificar que el producto existe y tiene stock
            if (!await _productoRepository.HasStockAsync(productoId))
                return false;

            // Obtener o crear carrito activo
            var carrito = await GetOrCreateCarritoForUsuarioAsync(usuarioId);

            return await _carritoRepository.AddProductToCarritoAsync(
                carrito.id,
                productoId,
                cantidad,
                precioUnitario);
        }

        public async Task<bool> RemoveProductFromCarritoAsync(int usuarioId, int productoId)
        {
            var carrito = await _carritoRepository.GetActiveCarritoByUsuarioAsync(usuarioId);
            if (carrito == null)
                return false;

            return await _carritoRepository.RemoveProductFromCarritoAsync(carrito.id, productoId);
        }

        public async Task<bool> UpdateProductQuantityAsync(int usuarioId, int productoId, int cantidad)
        {
            var carrito = await _carritoRepository.GetActiveCarritoByUsuarioAsync(usuarioId);
            if (carrito == null)
                return false;

            return await _carritoRepository.UpdateProductQuantityAsync(carrito.id, productoId, cantidad);
        }

        public async Task<bool> ClearCarritoAsync(int usuarioId)
        {
            var carrito = await _carritoRepository.GetActiveCarritoByUsuarioAsync(usuarioId);
            if (carrito == null)
                return false;

            return await _carritoRepository.ClearCarritoAsync(carrito.id);
        }

        public async Task<Carrito?> ConvertCarritoToPedidoAsync(int usuarioId)
        {
            var carrito = await _carritoRepository.GetActiveCarritoWithProductsAsync(usuarioId);
            if (carrito == null || !carrito.CarritoProductos.Any())
                return null;

            // Verificar stock de todos los productos
            foreach (var item in carrito.CarritoProductos)
            {
                if (!await _productoRepository.HasStockAsync(item.productoId))
                    return null;
            }

            return await _carritoRepository.ConvertCarritoToPedidoAsync(carrito.id);
        }

        public async Task<bool> RemoveAllInactiveCarritosAsync(int usuarioId)
        {
            return await _carritoRepository.RemoveAllInactiveCarritosAsync(usuarioId);
        }

        public async Task<bool> MergeCarritosAsync(int usuarioId, int carritoInvitadoId)
        {
            try
            {
                // Obtener carritos
                var carritoPrincipal = await _carritoRepository.GetActiveCarritoWithProductsAsync(usuarioId);
                var carritoInvitado = await _carritoRepository.GetCarritoWithProductsAsync(carritoInvitadoId);

                if (carritoInvitado == null || !carritoInvitado.CarritoProductos.Any())
                    return true;

                // Si no tiene carrito activo, asignar el invitado
                if (carritoPrincipal == null)
                {
                    carritoInvitado.usuarioId = usuarioId;
                    carritoInvitado.estado = "activo";
                    carritoInvitado.updatedAt = DateTime.Now;
                    _carritoRepository.Update(carritoInvitado);
                    return await _carritoRepository.SaveChangesAsync();
                }

                // Fusionar productos
                foreach (var itemInvitado in carritoInvitado.CarritoProductos)
                {
                    var itemExistente = carritoPrincipal.CarritoProductos.FirstOrDefault(cp => cp.productoId == itemInvitado.productoId);

                    if (itemExistente != null)
                    {
                        itemExistente.cantidad += itemInvitado.cantidad;
                        itemExistente.updatedAt = DateTime.Now;
                    }
                    else
                    {
                        itemInvitado.carritoId = carritoPrincipal.id;
                        carritoPrincipal.CarritoProductos.Add(itemInvitado);
                    }
                }

                carritoPrincipal.updatedAt = DateTime.Now;

                // Eliminar carrito invitado
                _carritoRepository.Remove(carritoInvitado);

                return await _carritoRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}