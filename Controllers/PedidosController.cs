using AutoMapper;
using computerChip.DTOs.Requests.Pedido;
using computerChip.DTOs.Responses.Pedido;
using computerChip.Extensions;
using computerChip.Models.Enum;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;

        public PedidosController(IPedidoService pedidoService, IMapper mapper)
        {
            _pedidoService = pedidoService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PedidoFilterRequest filter)
        {
            var pedidos = await _pedidoService.GetFilteredAsync(filter);
            return Ok(_mapper.Map<IEnumerable<PedidoListResponse>>(pedidos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            if (pedido == null) return NotFound();
            if (!User.IsInRole("Admin") && pedido.UsuarioId != User.GetUserId()) return Forbid();
            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            if (!User.IsInRole("Admin") && usuarioId != User.GetUserId()) return Forbid();
            var pedidos = await _pedidoService.GetByUsuarioAsync(usuarioId);
            return Ok(_mapper.Map<IEnumerable<PedidoListResponse>>(pedidos));
        }

        [HttpGet("estado/{estado}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEstado(string estado)
        {
            if (!Enum.TryParse<EstadoPedido>(estado, true, out var estadoEnum))
                return BadRequest("Estado inválido. Valores permitidos: PENDIENTE, CONFIRMADO, ENVIADO, ENTREGADO, CANCELADO");
            var pedidos = await _pedidoService.GetByEstadoAsync(estadoEnum);
            return Ok(_mapper.Map<IEnumerable<PedidoListResponse>>(pedidos));
        }

        [HttpGet("pendientes")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendientes()
        {
            var pedidos = await _pedidoService.GetPendingPedidosAsync();
            return Ok(_mapper.Map<IEnumerable<PedidoListResponse>>(pedidos));
        }

        [HttpGet("recientes")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRecientes([FromQuery] int days = 7)
        {
            if (days < 1 || days > 90) return BadRequest("El rango debe estar entre 1 y 90 días.");
            var pedidos = await _pedidoService.GetRecentPedidosAsync(days);
            return Ok(_mapper.Map<IEnumerable<PedidoListResponse>>(pedidos));
        }

        [HttpGet("stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new PedidoStatsResponse
            {
                TotalPedidos = await _pedidoService.GetTotalPedidosAsync(),
                PedidosPendientes = await _pedidoService.GetPedidosCountByEstadoAsync(EstadoPedido.PENDIENTE),
                PedidosConfirmados = await _pedidoService.GetPedidosCountByEstadoAsync(EstadoPedido.CONFIRMADO),
                PedidosEnviados = await _pedidoService.GetPedidosCountByEstadoAsync(EstadoPedido.ENVIADO),
                PedidosEntregados = await _pedidoService.GetPedidosCountByEstadoAsync(EstadoPedido.ENTREGADO),
                PedidosCancelados = await _pedidoService.GetPedidosCountByEstadoAsync(EstadoPedido.CANCELADO),
                TotalVentas = await _pedidoService.GetTotalVentasAsync(),
                PromedioVenta = await _pedidoService.GetPromedioVentaAsync(),
                MaxVenta = await _pedidoService.GetMaxVentaAsync(),
                PedidosHoy = (await _pedidoService.GetPedidosByDateRangeAsync(DateTime.Today, DateTime.Now)).Count()
            };
            return Ok(stats);
        }

        // Flujo tradicional de carrito: autenticado. El nuevo checkout usa /api/checkout/pedidos.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var usuarioId = User.GetUserId();
                var pedido = await _pedidoService.CreatePedidoFromCarritoAsync(usuarioId, request.MetodoPagoId, request.ZonaEnvioId);
                if (request.OfertaId.HasValue && request.OfertaId.Value > 0)
                {
                    pedido.OfertaId = request.OfertaId.Value;
                    await _pedidoService.UpdateEstadoAsync(pedido.id, pedido.estado);
                }
                var response = _mapper.Map<PedidoResponse>(pedido);
                return CreatedAtAction(nameof(GetById), new { id = pedido.id }, response);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception) { return StatusCode(500, new { error = "Error al crear el pedido" }); }
        }

        [HttpPut("{id}/confirmar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirmar(int id)
        {
            var result = await _pedidoService.ConfirmPedidoAsync(id);
            if (!result) return NotFound();
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }

        [HttpPut("{id}/enviar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enviar(int id)
        {
            var result = await _pedidoService.EnviarPedidoAsync(id);
            if (!result) return NotFound();
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }

        [HttpPut("{id}/entregar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Entregar(int id)
        {
            var result = await _pedidoService.EntregarPedidoAsync(id);
            if (!result) return NotFound();
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            if (pedido == null) return NotFound();
            var esAdmin = User.IsInRole("Admin");
            if (!esAdmin && pedido.UsuarioId != User.GetUserId()) return Forbid();

            // Un cliente solo puede cancelar mientras el pedido está pendiente.
            // El administrador puede cancelar pedidos pendientes o confirmados.
            if (!esAdmin && pedido.estado != EstadoPedido.PENDIENTE)
                return Conflict(new { error = "Solo podés cancelar un pedido pendiente." });

            if (pedido.estado != EstadoPedido.PENDIENTE &&
                pedido.estado != EstadoPedido.CONFIRMADO)
                return Conflict(new { error = "El pedido no puede cancelarse en su estado actual." });

            var result = await _pedidoService.CancelPedidoAsync(id);
            if (!result) return Conflict(new { error = "El pedido no puede cancelarse en su estado actual." });
            return NoContent();
        }
    }
}
