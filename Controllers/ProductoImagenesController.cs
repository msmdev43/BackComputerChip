using computerChip.DTOs.Responses.Productos;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/Productos/{productoId:int}/imagenes")]
    public sealed class ProductoImagenesController : ControllerBase
    {
        private readonly IProductoImagenesService _service;

        public ProductoImagenesController(IProductoImagenesService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductoImagenResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int productoId)
        {
            var result = await _service.GetByProductoAsync(productoId);
            if (result == null)
                return NotFound(new { error = "Producto no encontrado." });

            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(45 * 1024 * 1024)]
        [ProducesResponseType(typeof(IReadOnlyList<ProductoImagenResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Replace(int productoId)
        {
            if (!Request.HasFormContentType)
                return BadRequest(new { error = "La solicitud debe ser multipart/form-data." });

            var form = await Request.ReadFormAsync();
            var manifest = form["manifest"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(manifest))
                return BadRequest(new { error = "El campo manifest es obligatorio." });

            try
            {
                var result = await _service.ReplaceAsync(productoId, manifest, form.Files);
                if (result == null)
                    return NotFound(new { error = "Producto no encontrado." });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
