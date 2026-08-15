using AutoMapper;
using computerChip.DTOs.Requests.Pedido;
using computerChip.DTOs.Responses.Pedido;
using computerChip.Models.Enum;
using computerChip.Services.Implementations;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {

        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PedidoFilterRequest filter)
        {
            var pedidos = await _pedidoService.GetFilteredAsync(filter);
            var response = _mapper.Map<IEnumerable<PedidoListResponse>>(pedidos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            if (pedido == null)
                return NotFound();

            var response = _mapper.Map<PedidoResponse>(pedido);
            return Ok(response);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var pedidos = await _pedidoService.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<IEnumerable<PedidoListResponse>>(pedidos);
            return Ok(response);
        }

        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> GetByEstado(string estado)
        {
            if (!Enum.TryParse<EstadoPedido>(estado, true, out var estadoEnum))
                return BadRequest("Estado inválido. Valores permitidos: PENDIENTE, CONFIRMADO, ENVIADO, ENTREGADO, CANCELADO");

            var pedidos = await _pedidoService.GetByEstadoAsync(estadoEnum);
            var response = _mapper.Map<IEnumerable<PedidoListResponse>>(pedidos);
            return Ok(response);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var pedidos = await _pedidoService.GetPendingPedidosAsync();
            var response = _mapper.Map<IEnumerable<PedidoListResponse>>(pedidos);
            return Ok(response);
        }

        [HttpGet("recientes")]
        public async Task<IActionResult> GetRecientes([FromQuery] int days = 7)
        {
            var pedidos = await _pedidoService.GetRecentPedidosAsync(days);
            var response = _mapper.Map<IEnumerable<PedidoListResponse>>(pedidos);
            return Ok(response);
        }

        [HttpGet("stats")]
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = 1; // Obtener desde el usuario autenticado

            try
            {
                var pedido = await _pedidoService.CreatePedidoFromCarritoAsync(
                    usuarioId,
                    request.MetodoPagoId,
                    request.ZonaEnvioId
                );

                if (request.OfertaId.HasValue && request.OfertaId.Value > 0)
                {
                    pedido.OfertaId = request.OfertaId.Value;
                    await _pedidoService.UpdateEstadoAsync(pedido.id, pedido.estado);
                }

                var response = _mapper.Map<PedidoResponse>(pedido);
                return CreatedAtAction(nameof(GetById), new { id = pedido.id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al crear el pedido");
            }
        }

        [HttpPut("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(int id)
        {
            var result = await _pedidoService.ConfirmPedidoAsync(id);
            if (!result)
                return NotFound();

            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            var response = _mapper.Map<PedidoResponse>(pedido);
            return Ok(response);
        }

        [HttpPut("{id}/enviar")]
        public async Task<IActionResult> Enviar(int id)
        {
            var result = await _pedidoService.EnviarPedidoAsync(id);
            if (!result)
                return NotFound();

            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            var response = _mapper.Map<PedidoResponse>(pedido);
            return Ok(response);
        }

        [HttpPut("{id}/entregar")]
        public async Task<IActionResult> Entregar(int id)
        {
            var result = await _pedidoService.EntregarPedidoAsync(id);
            if (!result)
                return NotFound();

            var pedido = await _pedidoService.GetWithFullDetailsAsync(id);
            var response = _mapper.Map<PedidoResponse>(pedido);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var result = await _pedidoService.CancelPedidoAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}
