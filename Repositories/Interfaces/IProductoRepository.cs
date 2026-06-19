using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Productos>
    {
        Task<IEnumerable<Productos>> GetByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Productos>> GetByMarcaAsync(int marcaId);
        Task<IEnumerable<Productos>> GetByPrecioRangeAsync(decimal min, decimal max);
        Task<IEnumerable<Productos>> GetInStockAsync();
        Task<IEnumerable<Productos>> GetOnSaleAsync();
        Task<IEnumerable<Productos>> GetNewProductsAsync(int days);
        Task<IEnumerable<Productos>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Productos>> GetWithFullDetailsAsync();
        Task<Productos> GetWithFullDetailsByIdAsync(int id);
        Task<bool> HasStockAsync(int id, int cantidad);
        Task<bool> UpdateStockAsync(int id, int cantidad);
        Task<IEnumerable<Productos>> GetTopSellingAsync(int top);
        Task<IEnumerable<Productos>> GetRelatedProductsAsync(int productId);
        Task<decimal> GetAveragePriceAsync();
    }
}
