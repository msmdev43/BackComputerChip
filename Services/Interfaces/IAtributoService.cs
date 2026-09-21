using computerChip.DTOs.Responses.Atributo;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;

namespace computerChip.Services.Interfaces
{
    public interface IAtributoService
    {
        // CRUD
        Task<IEnumerable<Atributos>> GetAllActiveAsync();
        Task<Atributos?> GetByIdAsync(int id);
        Task<Atributos?> GetByNameAsync(string nombre);
        Task<Atributos?> GetWithProductosAsync(int id);
        Task<Atributos> CreateAtributoAsync(string nombre);
        Task<bool> UpdateAtributoAsync(int id, string nombre);

        /// <summary>
        /// Elimina un atributo SOLO si no tiene productos asociados.
        /// Devuelve false si tiene productos o si no existe.
        /// </summary>
        Task<bool> DeleteAtributoAsync(int id);

        // Relación producto ↔ atributo
        Task<bool> AddAtributoToProductoAsync(int productoId, int atributoId, string valor);
        Task<bool> RemoveAtributoFromProductoAsync(int productoId, int atributoId);
        Task<bool> UpdateAtributoValorAsync(int productoId, int atributoId, string valor);
        Task<IEnumerable<ProductosAtributos>> GetAtributosByProductoAsync(int productoId);
        Task<IEnumerable<ProductosAtributos>> GetAllAsignacionesAsync();

        // Compatibilidad
        Task<IEnumerable<ComparacionAtributoResponse>> CompararProductosAsync(List<int> productoIds);
        Task<IEnumerable<Productos>> GetProductosConMismoAtributoValorAsync(int atributoId, string valor);
    }
}