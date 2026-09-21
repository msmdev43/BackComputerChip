using computerChip.DTOs.Responses.Atributo;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;

namespace computerChip.Services.Implementations
{
    public class AtributoService : IAtributoService
    {
        private readonly IAtributoRepository _atributoRepository;

        public AtributoService(IAtributoRepository atributoRepository)
        {
            _atributoRepository = atributoRepository;
        }

        // ============================================
        // LECTURA
        // ============================================

        public async Task<IEnumerable<Atributos>> GetAllActiveAsync()
            => await _atributoRepository.GetAllActiveAsync();

        public async Task<Atributos?> GetByIdAsync(int id)
            => await _atributoRepository.GetByIdAsync(id);

        public async Task<Atributos?> GetByNameAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            return await _atributoRepository.GetByNameAsync(nombre);
        }

        public async Task<Atributos?> GetWithProductosAsync(int id)
            => await _atributoRepository.GetWithProductosAsync(id);

        // ============================================
        // CRUD
        // ============================================

        public async Task<Atributos> CreateAtributoAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del atributo es requerido");

            if (await _atributoRepository.ExistsByNameAsync(nombre))
                throw new InvalidOperationException($"Ya existe un atributo con el nombre '{nombre}'");

            var atributo = new Atributos { nombre = nombre.Trim() };
            await _atributoRepository.AddAsync(atributo);
            await _atributoRepository.SaveChangesAsync();
            return atributo;
        }

        public async Task<bool> UpdateAtributoAsync(int id, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return false;

            var atributo = await _atributoRepository.GetByIdAsync(id);
            if (atributo == null) return false;

            if (!atributo.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                if (await _atributoRepository.ExistsByNameAsync(nombre))
                    throw new InvalidOperationException($"Ya existe un atributo con el nombre '{nombre}'");
            }

            atributo.nombre = nombre.Trim();
            _atributoRepository.Update(atributo);
            return await _atributoRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Solo se puede eliminar un atributo si NO tiene productos asociados.
        /// Regla de negocio: los atributos definen compatibilidad, no se pueden
        /// borrar mientras estén en uso.
        /// </summary>
        public async Task<bool> DeleteAtributoAsync(int id)
        {
            var atributo = await _atributoRepository.GetByIdAsync(id);
            if (atributo == null) return false;

            // 🔒 Regla de negocio: no borrar si tiene productos asociados
            var count = await _atributoRepository.GetProductosCountAsync(id);
            if (count > 0)
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el atributo '{atributo.nombre}' porque está asociado a {count} producto(s). " +
                    "Primero desasociá el atributo de los productos."
                );
            }

            _atributoRepository.Remove(atributo);
            return await _atributoRepository.SaveChangesAsync();
        }

        // ============================================
        // RELACIÓN PRODUCTO ↔ ATRIBUTO
        // ============================================

        public async Task<bool> AddAtributoToProductoAsync(int productoId, int atributoId, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            return await _atributoRepository.AddAtributoToProductoAsync(productoId, atributoId, valor.Trim());
        }

        public async Task<bool> RemoveAtributoFromProductoAsync(int productoId, int atributoId)
            => await _atributoRepository.RemoveAtributoFromProductoAsync(productoId, atributoId);

        public async Task<bool> UpdateAtributoValorAsync(int productoId, int atributoId, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            return await _atributoRepository.UpdateAtributoValorAsync(productoId, atributoId, valor.Trim());
        }

        public async Task<IEnumerable<ProductosAtributos>> GetAtributosByProductoAsync(int productoId)
            => await _atributoRepository.GetAtributosByProductoAsync(productoId);

        // ============================================
        // COMPATIBILIDAD ENTRE PRODUCTOS
        // ============================================

        /// <summary>
        /// Compara N productos y devuelve, por cada atributo, el valor de cada
        /// producto + si son compatibles (todos con el mismo valor).
        /// </summary>
        public async Task<IEnumerable<ComparacionAtributoResponse>> CompararProductosAsync(List<int> productoIds)
        {
            if (productoIds == null || productoIds.Count < 2)
                return Enumerable.Empty<ComparacionAtributoResponse>();

            var relaciones = await _atributoRepository.GetAtributosByProductosAsync(productoIds);
            var listaRelaciones = relaciones.ToList();

            if (!listaRelaciones.Any())
                return Enumerable.Empty<ComparacionAtributoResponse>();

            // Agrupar por atributo
            var agrupado = listaRelaciones
                .GroupBy(pa => new { pa.atributoId, pa.Atributos.nombre });

            var resultado = new List<ComparacionAtributoResponse>();

            foreach (var grupo in agrupado)
            {
                var valoresPorProducto = new Dictionary<int, string?>();

                foreach (var productoId in productoIds)
                {
                    var relacion = grupo.FirstOrDefault(pa => pa.productoId == productoId);
                    valoresPorProducto[productoId] = relacion?.valor;
                }

                // Compatible si TODOS los productos tienen un valor no nulo y todos son iguales (case-insensitive)
                var valores = valoresPorProducto.Values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
                var sonCompatibles =
                    valores.Count == productoIds.Count &&
                    valores.Select(v => v!.Trim().ToLower()).Distinct().Count() == 1;

                resultado.Add(new ComparacionAtributoResponse
                {
                    AtributoId = grupo.Key.atributoId,
                    Nombre = grupo.Key.nombre,
                    ValoresPorProducto = valoresPorProducto,
                    SonCompatibles = sonCompatibles
                });
            }

            return resultado.OrderBy(r => r.Nombre);
        }

        public async Task<IEnumerable<Productos>> GetProductosConMismoAtributoValorAsync(int atributoId, string valor)
            => await _atributoRepository.GetProductosConMismoAtributoValorAsync(atributoId, valor);

        public async Task<IEnumerable<ProductosAtributos>> GetAllAsignacionesAsync() => await _atributoRepository.GetAllAsignacionesAsync();
    }
}