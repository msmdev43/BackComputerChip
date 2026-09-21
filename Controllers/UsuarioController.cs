using System.Security.Claims;
using AutoMapper;
using computerChip.DTOs.Responses.Usuario;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(
            IUsuarioService usuarioService,
            IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/usuario/getAll
        // Listar todos los usuarios activos
        // ⚠️ Ruta explícita "getAll" para que coincida con el front
        // ============================================
        [HttpGet("getAll")]
        
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllActiveAsync();
            var response = _mapper.Map<IEnumerable<UsuarioResponse>>(usuarios);
            return Ok(response);
        }

        // ============================================
        // GET: api/usuario/{id}
        // ============================================
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { Error = "Usuario no encontrado" });

            var usuarioDto = _mapper.Map<UsuarioResponse>(usuario);
            return Ok(usuarioDto);
        }

        // ============================================
        // GET: api/usuario/me
        // Usuario autenticado actual
        // ============================================
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { Error = "Token inválido" });

            var usuario = await _usuarioService.GetUserWithFullDetailsAsync(userId);
            if (usuario == null)
                return NotFound(new { Error = "Usuario no encontrado" });

            var response = _mapper.Map<UsuarioResponse>(usuario);
            return Ok(response);
        }

        // ============================================
        // GET: api/usuario/buscar?q=term
        // ============================================
        [HttpGet("buscar")]
        
        public async Task<IActionResult> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { Error = "El término de búsqueda es requerido" });

            var usuarios = await _usuarioService.SearchUsersAsync(q);
            var response = _mapper.Map<IEnumerable<UsuarioResponse>>(usuarios);
            return Ok(response);
        }

        // ============================================
        // GET: api/usuario/recientes?days=7
        // ============================================
        [HttpGet("recientes")]
        
        public async Task<IActionResult> GetRecientes([FromQuery] int days = 7)
        {
            if (days <= 0) days = 7;
            var usuarios = await _usuarioService.GetRecentUsersAsync(days);
            var response = _mapper.Map<IEnumerable<UsuarioResponse>>(usuarios);
            return Ok(response);
        }

        // ============================================
        // GET: api/usuario/stats
        // ============================================
        [HttpGet("stats")]
        
        public async Task<IActionResult> GetStats()
        {
            var total = await _usuarioService.GetTotalActiveUsersAsync();
            var recientes = await _usuarioService.GetRecentUsersAsync(7);

            return Ok(new
            {
                TotalUsuarios = total,
                NuevosUltimaSemana = recientes.Count()
            });
        }

        // ============================================
        // DELETE: api/usuario/{id}
        // Soft delete
        // ============================================
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _usuarioService.SoftDeleteUserAsync(id);
            if (!deleted)
                return NotFound(new { Error = "Usuario no encontrado o ya eliminado" });

            return NoContent();
        }
    }
}