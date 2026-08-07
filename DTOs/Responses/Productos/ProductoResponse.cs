namespace computerChip.DTOs.Responses.Productos
{
    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string Garantia { get; set; } = string.Empty;
        public bool Stock { get; set; }  // ✅ true = disponible, false = no disponible
        public string StockText => Stock ? "Disponible" : "No disponible";
        public bool EnvioGratis { get; set; }
        public string? CodigoSerie { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Categorias { get; set; } = new();
        public List<string> Marcas { get; set; } = new();
        public List<string> Imagenes { get; set; } = new();
        public List<EspecificacionResponse> Especificaciones { get; set; } = new();
        public List<AtributoResponse> Atributos { get; set; } = new();
        public bool IsOnSale => PrecioOferta.HasValue && PrecioOferta < Precio;
        public decimal? Descuento => IsOnSale ? Precio - PrecioOferta : null;
        public decimal? DescuentoPorcentaje => IsOnSale ? Math.Round(((Precio - PrecioOferta.Value) / Precio) * 100, 0) : null;
    }

    public class EspecificacionResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class AtributoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
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
}