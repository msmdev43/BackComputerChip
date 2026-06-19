using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface ICarritoRepository : IRepository<Carrito>
    {
        Task<Carrito> GetActiveCarritoByUsuarioAsync(int usuarioId);
        Task<Carrito> GetCarritoWithProductsAsync(int carritoId);
        Task<Carrito> GetActiveCarritoWithProductsAsync(int usuarioId);
        Task<bool> AddProductToCarritoAsync(int carritoId, int productoId, int cantidad, decimal precioUnitario);
        Task<bool> RemoveProductFromCarritoAsync(int carritoId, int productoId);
        Task<bool> UpdateProductQuantityAsync(int carritoId, int productoId, int cantidad);
        Task<bool> ClearCarritoAsync(int carritoId);
        Task<decimal> GetCarritoTotalAsync(int carritoId);
        Task<int> GetCarritoItemCountAsync(int carritoId);
        Task<Carrito> ConvertCarritoToPedidoAsync(int carritoId);
    }
}
