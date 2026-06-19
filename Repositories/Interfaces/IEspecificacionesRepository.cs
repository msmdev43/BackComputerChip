using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IEspecificacionesRepository : IRepository<Especificaciones>
    {
        Task<Especificaciones?> GetByTituloAsync(string titulo);
        Task<IEnumerable<Especificaciones>> GetByProductoAsync(int productoId);
        Task<IEnumerable<Especificaciones>> GetAllWithProductosAsync();
        Task<Especificaciones?> GetWithProductosByIdAsync(int id);
        Task<bool> ExistsByTituloAsync(string titulo);
    }
}