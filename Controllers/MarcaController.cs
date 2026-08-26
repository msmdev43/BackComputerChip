using AutoMapper;
using computerChip.DTOs.Requests.Marcas;
using computerChip.DTOs.Responses.Marcas;
using computerChip.Models;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService _marcaService;
        private readonly IMapper _mapper;

        public MarcaController(IMarcaService marcaService, IMapper mapper)
        {
            _marcaService = marcaService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/marca
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marcas = await _marcaService.GetAllActiveAsync();
            var response = _mapper.Map<IEnumerable<MarcaResponse>>(marcas);
            return Ok(response);
        }

        // ============================================
        // GET: api/marca/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var marca = await _marcaService.GetByIdAsync(id);
            if (marca == null)
                return NotFound($"No se encontró la marca con ID {id}");

            var response = _mapper.Map<MarcaResponse>(marca);
            return Ok(response);
        }

        // ============================================
        // GET: api/marca/{id}/detalle
        // ============================================
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetWithProductos(int id)
        {
            var marca = await _marcaService.GetWithProductosAsync(id);
            if (marca == null)
                return NotFound($"No se encontró la marca con ID {id}");

            var response = _mapper.Map<MarcaDetailResponse>(marca);
            return Ok(response);
        }

        // ============================================
        // GET: api/marca/buscar?nombre=...
        // ============================================
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByName([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre de la marca es requerido");

            var marca = await _marcaService.GetByNameAsync(nombre);
            if (marca == null)
                return NotFound($"No se encontró la marca '{nombre}'");

            var response = _mapper.Map<MarcaResponse>(marca);
            return Ok(response);
        }

        // ============================================
        // POST: api/marca
        // ============================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MarcaCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var marca = await _marcaService.CreateMarcaAsync(request.Nombre);
                var response = _mapper.Map<MarcaResponse>(marca);
                return CreatedAtAction(nameof(GetById), new { id = marca.id }, response);
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
        // PUT: api/marca/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MarcaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _marcaService.UpdateMarcaAsync(id, request.Nombre);
            if (!updated)
                return NotFound($"No se encontró la marca con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/marca/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _marcaService.SoftDeleteMarcaAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la marca con ID {id}");

            return NoContent();
        }

        // ============================================
        // PATCH: api/marca/{id}/restore
        // ============================================
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var restored = await _marcaService.RestoreMarcaAsync(id);
            if (!restored)
                return NotFound($"No se encontró la marca con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/marca/{id}/permanente
        // ============================================
        [HttpDelete("{id}/permanente")]
        public async Task<IActionResult> DeletePermanently(int id)
        {
            var deleted = await _marcaService.DeleteMarcaPermanentlyAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la marca con ID {id}");

            return NoContent();
        }
    }
}