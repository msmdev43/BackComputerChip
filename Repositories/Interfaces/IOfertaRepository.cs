using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IOfertaRepository : IRepository<Ofertas>
    {
        Task<IEnumerable<Ofertas>> GetAllActiveAsync();
        Task<IEnumerable<Ofertas>> GetByTipoAsync(string tipoOferta);
        Task<IEnumerable<Ofertas>> GetAllWithProductosAsync();
        Task<Ofertas?> GetWithProductosAsync(int id);
        Task<decimal> GetMaxDescuentoAsync();
        Task<IEnumerable<Ofertas>> GetOfertasVigentesAsync();
        Task<bool> HasActiveOfertaForProductoAsync(int productoId);
        Task<IEnumerable<Ofertas>> GetByDescuentoMayorAsync(decimal descuentoMinimo);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}