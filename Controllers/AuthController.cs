using AutoMapper;
using computerChip.DTOs.Requests.Admin;
using computerChip.DTOs.Requests.PushToken;
using computerChip.DTOs.Requests.Usuario;
using computerChip.DTOs.Responses.Auth;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        // ============================================
        // POST: api/auth/register
        // ============================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var (usuario, accessToken, refreshToken) = await _authService.RegisterAsync(
                    request.NombreCompleto ?? string.Empty,
                    request.Email,
                    request.Password,
                    request.Pais,
                    request.Provincia,
                    request.Ciudad,
                    request.Calle,
                    request.Numero,
                    request.Celular
                );

                var response = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600,
                    Usuario = _mapper.Map<UsuarioLoginResponse>(usuario)
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ============================================
        // POST: api/auth/login
        // ============================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (result == null)
                return Unauthorized(new { Error = "Credenciales inválidas" });

            var (usuario, accessToken, refreshToken) = result.Value;

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600,
                Usuario = _mapper.Map<UsuarioLoginResponse>(usuario)
            };

            return Ok(response);
        }

        // ============================================
        // POST: api/auth/login-google
        // ============================================
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] UsuarioGoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.LoginGoogleAsync(
                    request.GoogleSub,
                    request.Email,
                    request.Nombre,
                    request.AvatarUrl,
                    request.RefreshToken
                );

                if (result == null)
                    return Unauthorized(new { Error = "Error al autenticar con Google" });

                var (usuario, accessToken, refreshToken) = result.Value;

                var response = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600,
                    Usuario = _mapper.Map<UsuarioLoginResponse>(usuario)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ============================================
        // POST: api/auth/admin/login
        // ============================================
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AdminLoginAsync(request.Usuario, request.Password);
            if (result == null)
                return Unauthorized(new { Error = "Credenciales inválidas" });

            var (admin, accessToken, refreshToken) = result.Value;

            var response = new AdminLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600,
                Admin = _mapper.Map<AdminResponse>(admin)
            };

            return Ok(response);
        }

        // ============================================
        // POST: api/auth/refresh
        // ============================================
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { Error = "Refresh token es requerido" });

            var newAccessToken = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (newAccessToken == null)
                return Unauthorized(new { Error = "Refresh token inválido o expirado" });

            return Ok(new { AccessToken = newAccessToken });
        }

        // ============================================
        // POST: api/auth/logout
        // ============================================
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { Error = "Refresh token es requerido" });

            var result = await _authService.LogoutAsync(request.RefreshToken);
            if (!result)
                return BadRequest(new { Error = "No se pudo cerrar la sesión" });

            return Ok(new { Message = "Sesión cerrada exitosamente" });
        }

        // ============================================
        // POST: api/auth/logout/all
        // ============================================
        [HttpPost("logout/all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll([FromBody] LogoutAllRequest request)
        {
            if (request.UsuarioId <= 0)
                return BadRequest(new { Error = "ID de usuario es requerido" });

            var result = await _authService.LogoutAllAsync(request.UsuarioId);
            if (!result)
                return BadRequest(new { Error = "No se pudo cerrar todas las sesiones" });

            return Ok(new { Message = "Todas las sesiones cerradas exitosamente" });
        }

        // ============================================
        // POST: api/auth/verify-email
        // ============================================
        [HttpPost("verify-email")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail()
        {
            // Obtener usuario ID del token
            var usuarioIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del usuario" });

            var verified = await _authService.VerifyEmailAsync(usuarioId);
            if (!verified)
                return NotFound(new { Error = $"No se encontró el usuario con ID {usuarioId}" });

            return Ok(new { Message = "Email verificado exitosamente" });
        }

        // ============================================
        // POST: api/auth/send-verification
        // ============================================
        [HttpPost("send-verification")]
        [Authorize]
        public async Task<IActionResult> SendVerificationEmail()
        {
            // Obtener usuario ID del token
            var usuarioIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del usuario" });

            var sent = await _authService.SendVerificationEmailAsync(usuarioId);
            if (!sent)
                return BadRequest(new { Error = "No se pudo enviar el email de verificación" });

            return Ok(new { Message = "Email de verificación enviado exitosamente" });
        }

        // ============================================
        // GET: api/auth/validate
        // ============================================
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(new { Error = "Token es requerido" });

            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(new { Valid = isValid });
        }

        // ============================================
        // GET: api/auth/me
        // ============================================
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Obtener usuario ID del token
            var usuarioIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del usuario" });

            // Aquí se obtendría el usuario del servicio
            // var usuario = await _usuarioService.GetByIdAsync(usuarioId);
            // if (usuario == null) return NotFound();

            // var response = _mapper.Map<UsuarioLoginResponse>(usuario);
            // return Ok(response);

            return Ok(new { UsuarioId = usuarioId, Message = "Usuario autenticado" });
        }
    }
}