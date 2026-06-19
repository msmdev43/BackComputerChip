using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Productos>
    {
        Task<IEnumerable<Productos>> GetByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Productos>> GetByMarcaAsync(int marcaId);
        Task<IEnumerable<Productos>> GetByPrecioRangeAsync(decimal min, decimal max);
        Task<IEnumerable<Productos>> GetOnSaleAsync();
        Task<IEnumerable<Productos>> GetNewProductsAsync(int days);
        Task<IEnumerable<Productos>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Productos>> GetAllWithFullDetailsAsync();
        Task<Productos?> GetWithFullDetailsByIdAsync(int id);
        Task<Productos?> GetWithCategoriasMarcasAsync(int id);
        //Task<IEnumerable<Productos>> GetTopSellingAsync(int top);
        Task<IEnumerable<Productos>> GetRelatedProductsAsync(int productId);
        Task<decimal> GetAveragePriceAsync();
        Task<decimal> GetMinPriceAsync();
        Task<decimal> GetMaxPriceAsync();
        Task<int> GetTotalProductsAsync();
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}