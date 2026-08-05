using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface ICarritoService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Carrito?> GetActiveCarritoByUsuarioAsync(int usuarioId);
        Task<Carrito?> GetCarritoWithProductsAsync(int carritoId);
        Task<Carrito?> GetActiveCarritoWithProductsAsync(int usuarioId);
        Task<decimal> GetCarritoTotalAsync(int carritoId);
        Task<int> GetCarritoItemCountAsync(int carritoId);
        Task<int> GetCarritoDistinctProductsCountAsync(int carritoId);
        Task<bool> HasActiveCarritoAsync(int usuarioId);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Carrito> GetOrCreateCarritoForUsuarioAsync(int usuarioId);
        Task<bool> AddProductToCarritoAsync(int usuarioId, int productoId, int cantidad, decimal precioUnitario);
        Task<bool> RemoveProductFromCarritoAsync(int usuarioId, int productoId);
        Task<bool> UpdateProductQuantityAsync(int usuarioId, int productoId, int cantidad);
        Task<bool> ClearCarritoAsync(int usuarioId);
        Task<Carrito?> ConvertCarritoToPedidoAsync(int usuarioId);
        Task<bool> RemoveAllInactiveCarritosAsync(int usuarioId);
        Task<bool> MergeCarritosAsync(int usuarioId, int carritoInvitadoId);
    }
}