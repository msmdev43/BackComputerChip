using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IProductoService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Productos?> GetByIdAsync(int id);
        Task<IEnumerable<Productos>> GetAllActiveAsync();
        Task<IEnumerable<Productos>> GetByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Productos>> GetByMarcaAsync(int marcaId);
        Task<IEnumerable<Productos>> GetByPrecioRangeAsync(decimal min, decimal max);
        Task<IEnumerable<Productos>> GetInStockAsync();
        Task<IEnumerable<Productos>> GetOutOfStockAsync();
        Task<IEnumerable<Productos>> GetOnSaleAsync();
        Task<IEnumerable<Productos>> GetNewProductsAsync(int days);
        Task<IEnumerable<Productos>> SearchProductsAsync(string searchTerm);
        Task<Productos?> GetProductWithFullDetailsAsync(int id);
        Task<IEnumerable<Productos>> GetRelatedProductsAsync(int productId);
        Task<decimal> GetAveragePriceAsync();
        Task<decimal> GetMinPriceAsync();
        Task<decimal> GetMaxPriceAsync();
        Task<int> GetTotalProductsAsync();

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Productos> CreateProductAsync(Productos producto, List<int> categoriaIds, List<int> marcaIds);
        Task<bool> UpdateProductAsync(int id, Productos producto);
        Task<bool> UpdateStockAsync(int productoId, bool stock);
        Task<bool> SoftDeleteProductAsync(int id);
        Task<bool> RestoreProductAsync(int id);
        Task<bool> DeleteProductPermanentlyAsync(int id);
        Task<bool> AddCategoriesToProductAsync(int productoId, List<int> categoriaIds);
        Task<bool> RemoveCategoriesFromProductAsync(int productoId, List<int> categoriaIds);
        Task<bool> AddBrandsToProductAsync(int productoId, List<int> marcaIds);
        Task<bool> RemoveBrandsFromProductAsync(int productoId, List<int> marcaIds);
    }
}