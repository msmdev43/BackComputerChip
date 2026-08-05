using System.Collections.Generic;
using System.Threading.Tasks;
using computerChip.Models;

namespace computerChip.Services.Interfaces
{
    public interface IEspecificacionesService
    {
        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================
        Task<Especificaciones?> GetByIdAsync(int id);
        Task<Especificaciones?> GetByTituloAsync(string titulo);
        Task<IEnumerable<Especificaciones>> GetByProductoAsync(int productoId);
        Task<IEnumerable<Especificaciones>> GetAllWithProductosAsync();
        Task<Especificaciones?> GetWithProductosByIdAsync(int id);
        Task<bool> ExistsByTituloAsync(string titulo);

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================
        Task<Especificaciones> CreateEspecificacionAsync(string titulo, string descripcion);
        Task<bool> UpdateEspecificacionAsync(int id, string titulo, string descripcion);
        Task<bool> DeleteEspecificacionAsync(int id);
        Task<bool> AddToProductAsync(int especificacionId, int productoId);
        Task<bool> RemoveFromProductAsync(int especificacionId, int productoId);
        Task<bool> AddMultipleToProductAsync(int productoId, List<int> especificacionIds);
        Task<bool> RemoveMultipleFromProductAsync(int productoId, List<int> especificacionIds);
    }
}