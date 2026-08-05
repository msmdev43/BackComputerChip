using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IMarcaService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Marcas?> GetByIdAsync(int id);
        Task<Marcas?> GetByNameAsync(string nombre);
        Task<Marcas?> GetWithProductosAsync(int id);
        Task<IEnumerable<Marcas>> GetAllActiveAsync();
        Task<IEnumerable<Marcas>> GetAllWithProductosAsync();
        Task<int> GetProductCountByMarcaAsync(int marcaId);
        Task<bool> ExistsByNameAsync(string nombre);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Marcas> CreateMarcaAsync(string nombre);
        Task<bool> UpdateMarcaAsync(int id, string nombre);
        Task<bool> SoftDeleteMarcaAsync(int id);
        Task<bool> RestoreMarcaAsync(int id);
        Task<bool> DeleteMarcaPermanentlyAsync(int id);
    }
}