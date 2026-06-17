using System.ComponentModel.DataAnnotations.Schema;

namespace computerChip.Models.TablasIntermedias
{
    [Table("categorias_has_productos")]
    public class CategoriasProductos
    {
        [Column("categorias_idcategorias")]
        public int categoriaId { get; set; }

        [Column("productos_idproductos")]
        public int productoId { get; set; }

        // ============================================
        // RELACIONES
        // ============================================
        [ForeignKey(nameof(categoriaId))]
        public virtual Categorias Categorias { get; set; } = null!;

        [ForeignKey(nameof(productoId))]
        public virtual Productos Productos { get; set; } = null!;
    }
}
