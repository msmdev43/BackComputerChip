using computerChip.DTOs.Responses.Productos;
using Microsoft.AspNetCore.Http;

namespace computerChip.Services.Interfaces
{
    public interface IProductoImagenesService
    {
        Task<IReadOnlyList<ProductoImagenResponse>?> GetByProductoAsync(int productoId);
        Task<IReadOnlyList<ProductoImagenResponse>?> ReplaceAsync(
            int productoId,
            string manifestJson,
            IFormFileCollection files);
    }
}
