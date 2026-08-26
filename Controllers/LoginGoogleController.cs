using AutoMapper;
using computerChip.DTOs.Requests.Usuario;
using computerChip.DTOs.Responses.Auth;
using computerChip.DTOs.Responses.Usuario;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginGoogleController : ControllerBase
    {
        private readonly ILoginGoogleService _loginGoogleService;
        private readonly IMapper _mapper;

        public LoginGoogleController(ILoginGoogleService loginGoogleService, IMapper mapper)
        {
            _loginGoogleService = loginGoogleService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/logingoogle/usuario/{usuarioId}
        // ============================================
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var loginGoogle = await _loginGoogleService.GetByUsuarioIdAsync(usuarioId);
            if (loginGoogle == null)
                return NotFound($"No se encontró login de Google para el usuario con ID {usuarioId}");

            var response = _mapper.Map<UsuarioResponse>(loginGoogle);
            return Ok(response);
        }

        // ============================================
        // GET: api/logingoogle/google/{googleSub}
        // ============================================
        [HttpGet("google/{googleSub}")]
        public async Task<IActionResult> GetByGoogleSub(string googleSub)
        {
            var loginGoogle = await _loginGoogleService.GetByGoogleSubWithUsuarioAsync(googleSub);
            if (loginGoogle == null)
                return NotFound($"No se encontró usuario con Google Sub '{googleSub}'");

            var response = _mapper.Map<UsuarioResponse>(loginGoogle.Usuarios);
            return Ok(response);
        }

        // ============================================
        // POST: api/logingoogle
        // ============================================
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] UsuarioGoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loginGoogle = await _loginGoogleService.CreateOrUpdateGoogleLoginAsync(
                    request.GoogleSub,
                    request.Email,
                    request.Nombre,
                    request.AvatarUrl,
                    request.RefreshToken
                );

                var response = _mapper.Map<UsuarioResponse>(loginGoogle.Usuarios);
                return Ok(new
                {
                    Usuario = response,
                    Message = "Login con Google exitoso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ============================================
        // PATCH: api/logingoogle/{id}/lastlogin
        // ============================================
        [HttpPatch("{id}/lastlogin")]
        public async Task<IActionResult> UpdateLastLogin(int id)
        {
            var updated = await _loginGoogleService.UpdateLastLoginAsync(id);
            if (!updated)
                return NotFound($"No se encontró el login de Google con ID {id}");

            return NoContent();
        }

        // ============================================
        // PATCH: api/logingoogle/{id}/refresh
        // ============================================
        [HttpPatch("{id}/refresh")]
        public async Task<IActionResult> UpdateRefreshToken(int id, [FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("El refresh token no puede estar vacío");

            var updated = await _loginGoogleService.UpdateRefreshTokenAsync(id, refreshToken);
            if (!updated)
                return NotFound($"No se encontró el login de Google con ID {id}");

            return NoContent();
        }

        // ============================================
        // DELETE: api/logingoogle/usuario/{usuarioId}
        // ============================================
        [HttpDelete("usuario/{usuarioId}")]
        public async Task<IActionResult> SoftDeleteByUsuarioId(int usuarioId)
        {
            var deleted = await _loginGoogleService.SoftDeleteByUsuarioIdAsync(usuarioId);
            if (!deleted)
                return NotFound($"No se encontró login de Google para el usuario con ID {usuarioId}");

            return NoContent();
        }
    }
}