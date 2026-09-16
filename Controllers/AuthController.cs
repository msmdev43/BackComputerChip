using AutoMapper;
using computerChip.DTOs.Requests.Admin;
using computerChip.DTOs.Requests.PushToken;
using computerChip.DTOs.Requests.Usuario;
using computerChip.DTOs.Responses.Auth;
using computerChip.Services.Interfaces;
using Google.Apis.Auth;
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
        private readonly IConfiguration _configuration;

        public AuthController(
            IAuthService authService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _authService = authService;
            _mapper = mapper;
            _configuration = configuration;
        }

        // ============================================
        // POST: api/auth/login-google
        // Login/Registro de USUARIOS con Google
        // ============================================
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.IdToken))
                return BadRequest(new { Error = "El token de Google es requerido" });

            try
            {
                // 🔥 Validar el ID Token con Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    });

                if (payload == null)
                    return Unauthorized(new { Error = "Token de Google inválido" });

                // Extraer datos del token
                var googleSub = payload.Subject;
                var email = payload.Email;
                var nombre = payload.Name;
                var avatarUrl = payload.Picture;

                // Autenticar/crear usuario
                var result = await _authService.LoginGoogleAsync(
                    googleSub,
                    email,
                    nombre ?? "",
                    avatarUrl,
                    null);

                if (result == null)
                    return Unauthorized(new { Error = "Error al autenticar con Google" });

                var (usuario, accessToken, refreshToken) = result.Value;

                return Ok(new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600,
                    Usuario = _mapper.Map<UsuarioLoginResponse>(usuario)
                });
            }
            catch (InvalidJwtException ex)
            {
                Console.WriteLine($"❌ Token de Google inválido: {ex.Message}");
                return Unauthorized(new { Error = "Token de Google inválido" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en login Google: {ex.Message}");
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ============================================
        // POST: api/auth/admin/login
        // Login de ADMINISTRADOR con usuario + password
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

            return Ok(new AdminLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600,
                Admin = _mapper.Map<AdminResponse>(admin)
            });
        }

        // ============================================
        // POST: api/auth/refresh
        // Renovar access token usando refresh token
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
        // Cerrar sesión (revoca refresh token)
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
        // Cerrar TODAS las sesiones de un usuario
        // ============================================
        [HttpPost("logout/all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            // Obtener usuario ID del token
            var usuarioIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del usuario" });

            var result = await _authService.LogoutAllAsync(usuarioId);
            if (!result)
                return BadRequest(new { Error = "No se pudo cerrar todas las sesiones" });

            return Ok(new { Message = "Todas las sesiones cerradas exitosamente" });
        }

        // ============================================
        // GET: api/auth/me
        // Obtener datos del usuario autenticado
        // ============================================
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var usuarioIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del usuario" });

            return Ok(new
            {
                UsuarioId = usuarioId,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Nombre = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                Rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }

    // ============================================
    // REQUEST DTOs
    // ============================================
    public class GoogleTokenRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}