using AutoMapper;
using computerChip.DTOs.Requests.Especificacion;
using computerChip.DTOs.Responses.Especificacion;
using computerChip.Models;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecificacionController : ControllerBase
    {
        private readonly IEspecificacionesService _especificacionService;
        private readonly IMapper _mapper;

        public EspecificacionController(IEspecificacionesService especificacionService, IMapper mapper)
        {
            _especificacionService = especificacionService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/especificacion
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var especificaciones = await _especificacionService.GetAllWithProductosAsync();
            var response = _mapper.Map<IEnumerable<EspecificacionResponse>>(especificaciones);
            return Ok(response);
        }

        // ============================================
        // GET: api/especificacion/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var especificacion = await _especificacionService.GetByIdAsync(id);
            if (especificacion == null)
                return NotFound($"No se encontró la especificación con ID {id}");

            var response = _mapper.Map<EspecificacionResponse>(especificacion);
            return Ok(response);
        }

        // ============================================
        // GET: api/especificacion/producto/{productoId}
        // ============================================
        [HttpGet("producto/{productoId}")]
        public async Task<IActionResult> GetByProducto(int productoId)
        {
            var especificaciones = await _especificacionService.GetByProductoAsync(productoId);
            var response = _mapper.Map<IEnumerable<EspecificacionResponse>>(especificaciones);
            return Ok(response);
        }

        // ============================================
        // GET: api/especificacion/buscar?titulo=...
        // ============================================
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByTitulo([FromQuery] string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return BadRequest("El título de la especificación es requerido");

            var especificacion = await _especificacionService.GetByTituloAsync(titulo);
            if (especificacion == null)
                return NotFound($"No se encontró la especificación '{titulo}'");

            var response = _mapper.Map<EspecificacionResponse>(especificacion);
            return Ok(response);
        }

        // ============================================
        // POST: api/especificacion
        // ============================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EspecificacionCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var especificacion = await _especificacionService.CreateEspecificacionAsync(
                    request.Titulo,
                    request.Descripcion
                );
                var response = _mapper.Map<EspecificacionResponse>(especificacion);
                return CreatedAtAction(nameof(GetById), new { id = especificacion.id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ============================================
        // PUT: api/especificacion/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EspecificacionUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _especificacionService.UpdateEspecificacionAsync(
                id,
                request.Titulo ?? string.Empty,
                request.Descripcion ?? string.Empty
            );
            if (!updated)
                return NotFound($"No se encontró la especificación con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/especificacion/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _especificacionService.DeleteEspecificacionAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la especificación con ID {id}");

            return NoContent();
        }

        // ============================================
        // POST: api/especificacion/asignar
        // ============================================
        [HttpPost("asignar")]
        public async Task<IActionResult> AddToProduct([FromQuery] int especificacionId, [FromQuery] int productoId)
        {
            var added = await _especificacionService.AddToProductAsync(especificacionId, productoId);
            if (!added)
                return BadRequest("No se pudo asignar la especificación al producto");

            return Ok(new { Message = "Especificación asignada correctamente" });
        }

        // ============================================
        // DELETE: api/especificacion/asignar
        // ============================================
        [HttpDelete("asignar")]
        public async Task<IActionResult> RemoveFromProduct([FromQuery] int especificacionId, [FromQuery] int productoId)
        {
            var removed = await _especificacionService.RemoveFromProductAsync(especificacionId, productoId);
            if (!removed)
                return BadRequest("No se pudo desasignar la especificación del producto");

            return Ok(new { Message = "Especificación desasignada correctamente" });
        }
    }
}