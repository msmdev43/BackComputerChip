using computerChip.DTOs.Requests.Checkout;
using computerChip.Extensions;
using computerChip.Services.Implementations;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICotizacionService _cotizacionService;
        private readonly ICheckoutPedidoService _checkoutPedidoService;

        public CheckoutController(
            ICotizacionService cotizacionService,
            ICheckoutPedidoService checkoutPedidoService)
        {
            _cotizacionService = cotizacionService;
            _checkoutPedidoService = checkoutPedidoService;
        }

        [HttpPost("cotizacion")]
        public async Task<IActionResult> CrearCotizacion(
            [FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioId = User.GetUserId();

                var cotizacion = await _cotizacionService
                    .CrearCotizacionAsync(usuarioId, request);

                return Ok(cotizacion);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = "Ocurrió un error al generar la cotización."
                });
            }
        }

        [HttpPost("pedidos")]
        public async Task<IActionResult> CrearPedido(
            [FromBody] CrearPedidoRequest request,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioId = User.GetUserId();

                var pedido = await _checkoutPedidoService.CrearPedidoAsync(
                    usuarioId,
                    request,
                    idempotencyKey);

                return Ok(pedido);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (CheckoutConflictException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = "Ocurrió un error al crear el pedido."
                });
            }
        }
    }
}