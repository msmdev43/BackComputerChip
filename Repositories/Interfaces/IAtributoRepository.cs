using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Implementations;

namespace computerChip.Repositories.Interfaces
{
    public interface IAtributoRepository : IRepository<Atributos>
    {
        Task<IEnumerable<Atributos>> GetAllActiveAsync();
        Task<Atributos?> GetByNameAsync(string nombre);
        Task<Atributos?> GetWithProductosAsync(int id);
        Task<bool> ExistsByNameAsync(string nombre);
        Task<int> GetProductosCountAsync(int atributoId);

        // Relación producto ↔ atributo
        Task<bool> AddAtributoToProductoAsync(int productoId, int atributoId, string valor);
        Task<bool> RemoveAtributoFromProductoAsync(int productoId, int atributoId);
        Task<bool> UpdateAtributoValorAsync(int productoId, int atributoId, string valor);
        Task<IEnumerable<ProductosAtributos>> GetAtributosByProductoAsync(int productoId);
        Task<IEnumerable<ProductosAtributos>> GetAllAsignacionesAsync();

        // Comparación / compatibilidad
        Task<IEnumerable<ProductosAtributos>> GetAtributosByProductosAsync(List<int> productoIds);
        Task<IEnumerable<Productos>> GetProductosConMismoAtributoValorAsync(int atributoId, string valor);
    }
}
