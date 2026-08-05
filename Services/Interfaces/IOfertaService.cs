using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IOfertaService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Ofertas?> GetByIdAsync(int id);
        Task<Ofertas?> GetWithProductosAsync(int id);
        Task<IEnumerable<Ofertas>> GetAllActiveAsync();
        Task<IEnumerable<Ofertas>> GetByTipoAsync(string tipoOferta);
        Task<IEnumerable<Ofertas>> GetAllWithProductosAsync();
        Task<IEnumerable<Ofertas>> GetOfertasVigentesAsync();
        Task<IEnumerable<Ofertas>> GetByDescuentoMayorAsync(decimal descuentoMinimo);
        Task<decimal> GetMaxDescuentoAsync();
        Task<bool> HasActiveOfertaForProductoAsync(int productoId);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Ofertas> CreateOfertaAsync(Ofertas oferta, List<int> productoIds);
        Task<bool> UpdateOfertaAsync(int id, Ofertas oferta);
        Task<bool> AddProductsToOfertaAsync(int ofertaId, List<int> productoIds);
        Task<bool> RemoveProductsFromOfertaAsync(int ofertaId, List<int> productoIds);
        Task<bool> SoftDeleteOfertaAsync(int id);
        Task<bool> RestoreOfertaAsync(int id);
        Task<bool> DeleteOfertaPermanentlyAsync(int id);
        Task<bool> ApplyOfertaToProductAsync(int productoId, int ofertaId);
        Task<bool> RemoveOfertaFromProductAsync(int productoId);
    }
}