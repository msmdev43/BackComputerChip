using computerChip.DTOs.Responses.Productos;

namespace computerChip.DTOs.Responses.Atributo
{
    public class AtributoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class AtributoDetailResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<ProductoMiniResponse> Productos { get; set; } = new();
        public int ProductosCount => Productos.Count;
    }

    public class ComparacionAtributoResponse
    {
        public int AtributoId { get; set; }
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Valor del atributo en cada producto (key = productoId).
        /// </summary>
        public Dictionary<int, string?> ValoresPorProducto { get; set; } = new();

        /// <summary>
        /// True si TODOS los productos tienen el mismo valor en este atributo.
        /// Esto indica compatibilidad.
        /// </summary>
        public bool SonCompatibles { get; set; }
    }

    public class AsignacionAtributoResponse
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;

        public int AtributoId { get; set; }
        public string AtributoNombre { get; set; } = string.Empty;

        public string Valor { get; set; } = string.Empty;
    }
}