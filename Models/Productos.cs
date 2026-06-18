using computerChip.Models.TablasIntermedias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models
{
    [Table("productos")]
    public class Productos
    {
        [Key]
        public int id { get; set; }

        [Column("nombre")]
        [MaxLength(255)]
        [Required]
        public string nombre { get; set; } = string.Empty;

        [Column("precio")]
        [Required]
        public decimal precio { get; set; }
        [Column("precio_oferta")]
        public decimal? precioOferta { get; set; }

        [Column("garantia")]
        [MaxLength(85)]
        [Required]
        public string garantia { get; set; } = string.Empty;

        [Column("stock")]
        public Boolean stock { get; set; }

        [Column("envioGratis")]
        public int envioGratis { get; set; }

        [Column("codigoSerie")]
        [MaxLength(85)]
        public string? codigoSerie { get; set; }
        [Column("createdAt")]
        public DateTime createdAt { get; set; }

        [Column("updatedAt")]
        public DateTime updatedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? deletedAt { get; set; }

        // ============================================
        // RELACIONES (Many-to-Many con tablas intermedias)
        // ============================================
        public virtual ICollection<CategoriasProductos> CategoriasProductos { get; set; } = new List<CategoriasProductos>();
        public virtual ICollection<ProductosMarcas> ProductosMarcas { get; set; } = new List<ProductosMarcas>();
        public virtual ICollection<ProductosImagenes> ProductosImagenes { get; set; } = new List<ProductosImagenes>();
        public virtual ICollection<ProductosOfertas> ProductosOfertas { get; set; } = new List<ProductosOfertas>();
        public virtual ICollection<ProductosPreguntas> ProductosPreguntas { get; set; } = new List<ProductosPreguntas>();
        public virtual ICollection<ProductosEspecificaciones> ProductosEspecificaciones { get; set; } = new List<ProductosEspecificaciones>();
        public virtual ICollection<CarritoProductos> CarritoProductos { get; set; } = new List<CarritoProductos>();
        public virtual ICollection<ItemPedidoProductos> ItemsPedidoProductos { get; set; } = new List<ItemPedidoProductos>();

        // Relación con atributos (tabla intermedia con valor)
        public virtual ICollection<ProductosAtributos> ProductoAtributos { get; set; } = new List<ProductosAtributos>();
    }
}
