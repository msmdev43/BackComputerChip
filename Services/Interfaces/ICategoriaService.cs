using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface ICategoriaService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Categorias?> GetByIdAsync(int id);
        Task<Categorias?> GetByNameAsync(string nombre);
        Task<Categorias?> GetWithProductosAsync(int id);
        Task<IEnumerable<Categorias>> GetAllActiveAsync();
        Task<IEnumerable<Categorias>> GetAllWithProductosAsync();
        Task<int> GetProductCountByCategoriaAsync(int categoriaId);
        Task<bool> ExistsByNameAsync(string nombre);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Categorias> CreateCategoriaAsync(string nombre);
        Task<bool> UpdateCategoriaAsync(int id, string nombre);
        Task<bool> SoftDeleteCategoriaAsync(int id);
        Task<bool> RestoreCategoriaAsync(int id);
        Task<bool> DeleteCategoriaPermanentlyAsync(int id);
    }
}