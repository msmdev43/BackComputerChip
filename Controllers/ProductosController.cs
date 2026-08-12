using AutoMapper;
using computerChip.DTOs.Requests.Productos;
using computerChip.DTOs.Responses.Productos;
using computerChip.Models;
using computerChip.Services.Implementations;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService productoService;
        private readonly IMapper mapper;

        public ProductosController(IProductoService productoService, IMapper mapper)
        {
            this.productoService = productoService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetAll()
        {
            var productos = await productoService.GetAllActiveAsync();
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetById(int id)
        {
            var producto = await productoService.GetProductWithFullDetailsAsync(id);
            if (producto == null)
                return NotFound($"Producto con ID {id} no encontrado");

            var response = mapper.Map<ProductoResponse>(producto);
            return Ok(response);
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetByCategoria(int categoriaId)
        {
            var productos = await productoService.GetByCategoriaAsync(categoriaId);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("marca/{marcaId}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetByMarca(int marcaId)
        {
            var productos = await productoService.GetByMarcaAsync(marcaId);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("precio")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetByPrecioRange([FromQuery] decimal min, [FromQuery] decimal max)
        {
            if (min < 0 || max < 0 || min > max)
                return BadRequest("Los valores de precio no son válidos");

            var productos = await productoService.GetByPrecioRangeAsync(min, max);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("stock/{inStock}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetByStock(bool inStock)
        {
            IEnumerable<Productos> productos;
            if (inStock)
                productos = await productoService.GetInStockAsync();
            else
                productos = await productoService.GetOutOfStockAsync();

            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("oferta")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetOnSale()
        {
            var productos = await productoService.GetOnSaleAsync();
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("nuevos")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetNewProducts([FromQuery] int days = 7)
        {
            if (days <= 0) days = 7;
            var productos = await productoService.GetNewProductsAsync(days);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("El término de búsqueda no puede estar vacío");

            var productos = await productoService.SearchProductsAsync(q);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }

        [HttpGet("relacionados/{id}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetRelated(int id)
        {
            var productos = await productoService.GetRelatedProductsAsync(id);
            var response = mapper.Map<IEnumerable<ProductoResponse>>(productos);
            return Ok(response);
        }


        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            var total = await productoService.GetTotalProductsAsync();
            var avg = await productoService.GetAveragePriceAsync();
            var min = await productoService.GetMinPriceAsync();
            var max = await productoService.GetMaxPriceAsync();

            return Ok(new
            {
                TotalProductos = total,
                PrecioPromedio = avg,
                PrecioMinimo = min,
                PrecioMaximo = max
            });
        }


        [HttpPost]
        public async Task<ActionResult<ProductoResponse>> Create([FromBody] ProductoCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = mapper.Map<Productos>(request);

            var created = await productoService.CreateProductAsync(
                producto,
                request.CategoriaIds ?? new List<int>(),
                request.MarcaIds ?? new List<int>()
            );

            var response = mapper.Map<ProductoResponse>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await productoService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Producto con ID {id} no encontrado");

            mapper.Map(request, existing);

            var success = await productoService.UpdateProductAsync(id, existing);
            if (!success)
                return StatusCode(500, "Error al actualizar el producto");

            var response = mapper.Map<ProductoResponse>(existing);
            return Ok(response);
        }


        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] bool stock)
        {
            var success = await productoService.UpdateStockAsync(id, stock);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o no se pudo actualizar");

            return Ok(new { id, stock, mensaje = "Stock actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await productoService.SoftDeleteProductAsync(id);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o ya eliminado");

            return NoContent();
        }

        [HttpPost("{id}/restaurar")]
        public async Task<IActionResult> Restore(int id)
        {
            var success = await productoService.RestoreProductAsync(id);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o no está eliminado");

            return Ok(new { mensaje = "Producto restaurado correctamente" });
        }

        [HttpPost("{id}/categorias")]
        public async Task<IActionResult> AddCategories(int id, [FromBody] List<int> categoriaIds)
        {
            if (categoriaIds == null || !categoriaIds.Any())
                return BadRequest("Debe proporcionar al menos un ID de categoría");

            var success = await productoService.AddCategoriesToProductAsync(id, categoriaIds);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o error al agregar categorías");

            return Ok(new { mensaje = "Categorías agregadas correctamente" });
        }

        [HttpDelete("{id}/categorias")]
        public async Task<IActionResult> RemoveCategories(int id, [FromBody] List<int> categoriaIds)
        {
            if (categoriaIds == null || !categoriaIds.Any())
                return BadRequest("Debe proporcionar al menos un ID de categoría");

            var success = await productoService.RemoveCategoriesFromProductAsync(id, categoriaIds);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o error al eliminar categorías");

            return Ok(new { mensaje = "Categorías eliminadas correctamente" });
        }


        [HttpPost("{id}/marcas")]
        public async Task<IActionResult> AddBrands(int id, [FromBody] List<int> marcaIds)
        {
            if (marcaIds == null || !marcaIds.Any())
                return BadRequest("Debe proporcionar al menos un ID de marca");

            var success = await productoService.AddBrandsToProductAsync(id, marcaIds);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o error al agregar marcas");

            return Ok(new { mensaje = "Marcas agregadas correctamente" });
        }

        [HttpDelete("{id}/marcas")]
        public async Task<IActionResult> RemoveBrands(int id, [FromBody] List<int> marcaIds)
        {
            if (marcaIds == null || !marcaIds.Any())
                return BadRequest("Debe proporcionar al menos un ID de marca");

            var success = await productoService.RemoveBrandsFromProductAsync(id, marcaIds);
            if (!success)
                return NotFound($"Producto con ID {id} no encontrado o error al eliminar marcas");

            return Ok(new { mensaje = "Marcas eliminadas correctamente" });
        }
    }
}
