using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categorias>
    {
        Task<IEnumerable<Categorias>> GetAllActiveAsync();
        Task<Categorias?> GetByNameAsync(string nombre);
        Task<Categorias?> GetWithProductosAsync(int id);
        Task<IEnumerable<Categorias>> GetAllWithProductosAsync();
        Task<int> GetProductCountByCategoriaAsync(int categoriaId);
        Task<bool> ExistsByNameAsync(string nombre);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}
