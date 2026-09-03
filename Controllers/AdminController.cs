using AutoMapper;
using computerChip.DTOs.Requests.Admin;
using computerChip.DTOs.Responses.AdminDashboard;
using computerChip.Services;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;


        public AdminController(IAdminService adminService, IMapper mapper, JwtService jwtService)
        {
            _adminService = adminService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        // ============================================
        // POST: api/admin/login
        // ============================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admin = await _adminService.AuthenticateAsync(request.Usuario, request.Password);
            if (admin == null)
                return Unauthorized(new { Error = "Credenciales inválidas" });

            var token = _jwtService.GenerateToken(admin.id, admin.usuario);

            var response = _mapper.Map<AdminResponse>(admin);
                response.Token = token;
            return Ok(response);
        }

        // ============================================
        // GET: api/admin/dashboard/stats
        // ============================================
        [HttpGet("dashboard/stats")]
      
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = new DashboardStatsResponse
            {
                // Pedidos
                TotalPedidos = await _adminService.GetTotalPedidosAsync(),
                PedidosHoy = await _adminService.GetPedidosHoyAsync(),
                PedidosPendientes = 0, // Pendiente de implementar
                ProductosPedidos = await _adminService.GetCantidadPedidos(),

                // Usuarios
                TotalUsuarios = await _adminService.GetTotalUsuariosAsync(),
                NuevosUsuariosMes = 0, // Pendiente de implementar
                UsuariosGoogle = 0, // Pendiente de implementar

                // Productos
                TotalProductos = 0, // Pendiente de implementar
                ProductosSinStock = 0, // Pendiente de implementar
                TotalCategorias = await _adminService.GetTotalCategoriasAsync(),
                ProductosConCategoria = await _adminService.GetCantidadProductosPorCategoriaAsync(),

                // Ventas
                VentasTotales = 0, // Pendiente de implementar
                VentasMes = 0, // Pendiente de implementar
                VentasSemana = 0, // Pendiente de implementar
                PromedioVenta = 0, // Pendiente de implementar
                MaxVenta = 0, // Pendiente de implementar

                // Ofertas
                OfertasActivas = 0, // Pendiente de implementar
                DescuentoMaximo = 0 // Pendiente de implementar
            };

            return Ok(stats);
        }

        // ============================================
        // GET: api/admin/me
        // ============================================
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentAdmin()
        {
            var adminIdClaim = User.FindFirst("usuarioId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(adminIdClaim) || !int.TryParse(adminIdClaim, out var adminId))
                return Unauthorized(new { Error = "No se pudo obtener el ID del administrador" });

            var admin = await _adminService.GetByIdAsync(adminId);
            if (admin == null)
                return NotFound(new { Error = "Administrador no encontrado" });

            var response = _mapper.Map<AdminResponse>(admin);
            return Ok(response);
        }
    }
}