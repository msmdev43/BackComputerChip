using AutoMapper;
using computerChip.DTOs.Requests.ZonaEnvio;
using computerChip.DTOs.Responses.ZonaEnvio;
using computerChip.Models;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonaEnvioController : ControllerBase
    {
        private readonly IZonaEnvioService _zonaEnvioService;
        private readonly IMapper _mapper;

        public ZonaEnvioController(IZonaEnvioService zonaEnvioService, IMapper mapper)
        {
            _zonaEnvioService = zonaEnvioService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/zonaenvio
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var zonas = await _zonaEnvioService.GetAllActiveAsync();
            var response = _mapper.Map<IEnumerable<ZonaEnvioResponse>>(zonas);
            return Ok(response);
        }

        // ============================================
        // GET: api/zonaenvio/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var zona = await _zonaEnvioService.GetByIdAsync(id);
            if (zona == null)
                return NotFound($"No se encontró la zona de envío con ID {id}");

            var response = _mapper.Map<ZonaEnvioResponse>(zona);
            return Ok(response);
        }

        // ============================================
        // GET: api/zonaenvio/codigo/{codigoPostal}
        // ============================================
        [HttpGet("codigo/{codigoPostal}")]
        public async Task<IActionResult> GetByCodigoPostal(string codigoPostal)
        {
            var zona = await _zonaEnvioService.GetByCodigoPostalAsync(codigoPostal);
            if (zona == null)
                return NotFound($"No se encontró zona de envío para el código postal '{codigoPostal}'");

            var response = _mapper.Map<ZonaEnvioResponse>(zona);
            return Ok(response);
        }

        // ============================================
        // GET: api/zonaenvio/costo/{codigoPostal}
        // ============================================
        [HttpGet("costo/{codigoPostal}")]
        public async Task<IActionResult> GetCostoEnvio(string codigoPostal)
        {
            var costo = await _zonaEnvioService.GetCostoEnvioAsync(codigoPostal);
            var zona = await _zonaEnvioService.GetByCodigoPostalAsync(codigoPostal);

            var response = new ZonaEnvioCostoResponse
            {
                CodigoPostal = codigoPostal,
                Ciudad = zona?.ciudad ?? string.Empty,
                Provincia = zona?.provincia ?? string.Empty,
                Costo = costo,
                Disponible = costo > 0
            };

            return Ok(response);
        }

        // ============================================
        // GET: api/zonaenvio/pais/{pais}
        // ============================================
        [HttpGet("pais/{pais}")]
        public async Task<IActionResult> GetByPais(string pais)
        {
            var zonas = await _zonaEnvioService.GetByPaisAsync(pais);
            var response = _mapper.Map<IEnumerable<ZonaEnvioResponse>>(zonas);
            return Ok(response);
        }

        // ============================================
        // POST: api/zonaenvio
        // ============================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ZonaEnvioCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var zona = await _zonaEnvioService.CreateZonaEnvioAsync(
                    request.Ciudad,
                    request.Provincia,
                    request.Pais,
                    request.Costo,
                    request.CodigoPostal
                );
                var response = _mapper.Map<ZonaEnvioResponse>(zona);
                return CreatedAtAction(nameof(GetById), new { id = zona.id }, response);
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
        // PUT: api/zonaenvio/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ZonaEnvioUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _zonaEnvioService.UpdateZonaEnvioAsync(
                id,
                request.Ciudad,
                request.Provincia,
                request.Pais,
                request.Costo,
                request.CodigoPostal
            );
            if (!updated)
                return NotFound($"No se encontró la zona de envío con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/zonaenvio/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _zonaEnvioService.DeleteZonaEnvioAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la zona de envío con ID {id}");

            return NoContent();
        }

        // ============================================
        // PATCH: api/zonaenvio/{id}/costo
        // ============================================
        [HttpPatch("{id}/costo")]
        public async Task<IActionResult> UpdateCosto(int id, [FromBody] decimal costo)
        {
            if (costo < 0)
                return BadRequest("El costo no puede ser negativo");

            var updated = await _zonaEnvioService.UpdateCostoEnvioAsync(id, costo);
            if (!updated)
                return NotFound($"No se encontró la zona de envío con ID {id}");

            return NoContent();
        }
    }
}