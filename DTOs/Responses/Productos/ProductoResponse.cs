namespace computerChip.DTOs.Responses.Productos
{
    public class ProductoResponse
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public decimal precio { get; set; }
        public decimal? precioOferta { get; set; }
        public string garantia { get; set; } = string.Empty;
        public bool stock { get; set; }
        public int envioGratis { get; set; }
        public string? codigoSerie { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }

        public List<CategoriaResponse> Categorias { get; set; } = new();
        public List<MarcaResponse> Marcas { get; set; } = new();
        public List<ImagenResponse> Imagenes { get; set; } = new();
        public List<EspecificacionResponse> Especificaciones { get; set; } = new();
        public List<AtributoResponse> Atributos { get; set; } = new();
    }

    public class CategoriaResponse
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
    }

    public class MarcaResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class ImagenResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string url { get; set; }
    }

    public class EspecificacionResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class AtributoResponse
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
    }
}