using AutoMapper;
using computerChip.DTOs.Requests.Categoria;
using computerChip.DTOs.Responses.Categoria;
using computerChip.Models;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/categoria
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _categoriaService.GetAllActiveAsync();
            var response = _mapper.Map<IEnumerable<CategoriaResponse>>(categorias);
            return Ok(response);
        }

        // ============================================
        // GET: api/categoria/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
                return NotFound($"No se encontró la categoría con ID {id}");

            var response = _mapper.Map<CategoriaResponse>(categoria);
            return Ok(response);
        }

        // ============================================
        // GET: api/categoria/{id}/detalle
        // ============================================
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetWithProductos(int id)
        {
            var categoria = await _categoriaService.GetWithProductosAsync(id);
            if (categoria == null)
                return NotFound($"No se encontró la categoría con ID {id}");

            var response = _mapper.Map<CategoriaDetailResponse>(categoria);
            return Ok(response);
        }

        // ============================================
        // GET: api/categoria/buscar?nombre=...
        // ============================================
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByName([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre de la categoría es requerido");

            var categoria = await _categoriaService.GetByNameAsync(nombre);
            if (categoria == null)
                return NotFound($"No se encontró la categoría '{nombre}'");

            var response = _mapper.Map<CategoriaResponse>(categoria);
            return Ok(response);
        }

        // ============================================
        // POST: api/categoria
        // ============================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var categoria = await _categoriaService.CreateCategoriaAsync(request.Nombre);
                var response = _mapper.Map<CategoriaResponse>(categoria);
                return CreatedAtAction(nameof(GetById), new { id = categoria.id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // ============================================
        // PUT: api/categoria/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _categoriaService.UpdateCategoriaAsync(id, request.Nombre);
            if (!updated)
                return NotFound($"No se encontró la categoría con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/categoria/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoriaService.SoftDeleteCategoriaAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la categoría con ID {id}");

            return NoContent();
        }

        // ============================================
        // PATCH: api/categoria/{id}/restore
        // ============================================
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var restored = await _categoriaService.RestoreCategoriaAsync(id);
            if (!restored)
                return NotFound($"No se encontró la categoría con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/categoria/{id}/permanente
        // ============================================
        [HttpDelete("{id}/permanente")]
        public async Task<IActionResult> DeletePermanently(int id)
        {
            var deleted = await _categoriaService.DeleteCategoriaPermanentlyAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la categoría con ID {id}");

            return NoContent();
        }
    }
}