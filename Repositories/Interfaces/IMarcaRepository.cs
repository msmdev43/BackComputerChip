using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IMarcaRepository : IRepository<Marcas>
    {
        Task<IEnumerable<Marcas>> GetAllActiveAsync();
        Task<Marcas?> GetByNameAsync(string nombre);
        Task<Marcas?> GetWithProductosAsync(int id);
        Task<IEnumerable<Marcas>> GetAllWithProductosAsync();
        Task<int> GetProductCountByMarcaAsync(int marcaId);
        Task<bool> ExistsByNameAsync(string nombre);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}