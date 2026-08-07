// DTOs/Responses/Categoria/CategoriaResponse.cs
using computerChip.DTOs.Responses.Productos;

namespace computerChip.DTOs.Responses.Categoria
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ProductosCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoriaDetailResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ProductoMiniResponse> Productos { get; set; } = new();
        public int ProductosCount => Productos.Count;
    }
}