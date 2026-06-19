using computerChip.Models;

namespace computerChip.Repositories.Interfaces
{
    public interface IOfertaRepository : IRepository<Ofertas>
    {
        Task<IEnumerable<Ofertas>> GetActiveOfertasAsync();
        Task<IEnumerable<Ofertas>> GetByTipoAsync(string tipoOferta);
        Task<IEnumerable<Ofertas>> GetOfertasWithProductosAsync();
        Task<Ofertas> GetOfertaWithProductosAsync(int id);
        Task<decimal> GetMaxDescuentoAsync();
        Task<IEnumerable<Ofertas>> GetOfertasVigentesAsync();
        Task<bool> HasActiveOfertaForProductoAsync(int productoId);
    }
}
