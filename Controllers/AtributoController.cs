using AutoMapper;
using computerChip.DTOs.Requests.Atributo;
using computerChip.DTOs.Responses.Atributo;
using computerChip.DTOs.Responses.Productos;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace computerChip.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtributoController : ControllerBase
    {
        private readonly IAtributoService _atributoService;
        private readonly IMapper _mapper;

        public AtributoController(IAtributoService atributoService, IMapper mapper)
        {
            _atributoService = atributoService;
            _mapper = mapper;
        }

        // ============================================
        // GET: api/atributo
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var atributos = await _atributoService.GetAllActiveAsync();
            return Ok(_mapper.Map<IEnumerable<AtributoResponse>>(atributos));
        }

        // ============================================
        // GET: api/atributo/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var atributo = await _atributoService.GetByIdAsync(id);
            if (atributo == null)
                return NotFound(new { Error = $"No se encontró el atributo con ID {id}" });

            return Ok(_mapper.Map<AtributoResponse>(atributo));
        }

        // ============================================
        // GET: api/atributo/{id}/detalle
        // ============================================
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetWithProductos(int id)
        {
            var atributo = await _atributoService.GetWithProductosAsync(id);
            if (atributo == null)
                return NotFound(new { Error = $"No se encontró el atributo con ID {id}" });

            return Ok(_mapper.Map<AtributoDetailResponse>(atributo));
        }

        // ============================================
        // GET: api/atributo/buscar?nombre=...
        // ============================================
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByName([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest(new { Error = "El nombre es requerido" });

            var atributo = await _atributoService.GetByNameAsync(nombre);
            if (atributo == null)
                return NotFound(new { Error = $"No se encontró el atributo '{nombre}'" });

            return Ok(_mapper.Map<AtributoResponse>(atributo));
        }

        // ============================================
        // POST: api/atributo
        // ============================================
        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody] AtributoCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var atributo = await _atributoService.CreateAtributoAsync(request.Nombre);
                var response = _mapper.Map<AtributoResponse>(atributo);
                return CreatedAtAction(nameof(GetById), new { id = atributo.id }, response);
            }
            catch (InvalidOperationException ex) { return Conflict(new { Error = ex.Message }); }
            catch (ArgumentException ex) { return BadRequest(new { Error = ex.Message }); }
        }

        // ============================================
        // PUT: api/atributo/{id}
        // ============================================
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update(int id, [FromBody] AtributoUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var ok = await _atributoService.UpdateAtributoAsync(id, request.Nombre);
                if (!ok) return NotFound(new { Error = $"No se encontró el atributo con ID {id}" });

                return NoContent();
            }
            catch (InvalidOperationException ex) { return Conflict(new { Error = ex.Message }); }
        }

        // ============================================
        // DELETE: api/atributo/{id}
        // ⚠️ Falla si el atributo tiene productos asociados (409 Conflict)
        // ============================================
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _atributoService.DeleteAtributoAsync(id);
                if (!ok) return NotFound(new { Error = $"No se encontró el atributo con ID {id}" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Atributo en uso → no se puede borrar
                return Conflict(new { Error = ex.Message });
            }
        }

        // ============================================
        // RELACIÓN ATRIBUTO ↔ PRODUCTO
        // ============================================

        // POST: api/atributo/producto/{productoId}
        [HttpPost("producto/{productoId}")]
        
        public async Task<IActionResult> AsignarAProducto(int productoId, [FromBody] AsignarAtributosRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var asignados = new List<object>();
            var errores = new List<string>();

            foreach (var item in request.Atributos)
            {
                var ok = await _atributoService.AddAtributoToProductoAsync(
                    productoId, item.AtributoId, item.Valor);

                if (ok) asignados.Add(new { item.AtributoId, item.Valor });
                else errores.Add($"No se pudo asignar el atributo {item.AtributoId}");
            }

            if (errores.Any() && !asignados.Any())
                return BadRequest(new { Error = "No se pudo asignar ningún atributo", Detalles = errores });

            return Ok(new { Mensaje = "Proceso completado", Asignados = asignados, Errores = errores });
        }

        // PUT: api/atributo/producto/{productoId}/{atributoId}
        [HttpPut("producto/{productoId}/{atributoId}")]
        
        public async Task<IActionResult> ActualizarValor(int productoId, int atributoId, [FromBody] string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return BadRequest(new { Error = "El valor es requerido" });

            var ok = await _atributoService.UpdateAtributoValorAsync(productoId, atributoId, valor);
            if (!ok) return NotFound(new { Error = "No se encontró la relación producto-atributo" });

            return NoContent();
        }

        // DELETE: api/atributo/producto/{productoId}/{atributoId}
        [HttpDelete("producto/{productoId}/{atributoId}")]
        
        public async Task<IActionResult> QuitarDeProducto(int productoId, int atributoId)
        {
            var ok = await _atributoService.RemoveAtributoFromProductoAsync(productoId, atributoId);
            if (!ok) return NotFound(new { Error = "No se encontró la relación producto-atributo" });

            return NoContent();
        }

        // GET: api/atributo/producto/{productoId}
        [HttpGet("producto/{productoId}")]
        public async Task<IActionResult> GetByProducto(int productoId)
        {
            var atributos = await _atributoService.GetAtributosByProductoAsync(productoId);
            return Ok(_mapper.Map<IEnumerable<ProductoAtributoResponse>>(atributos));
        }

        // ============================================
        // COMPATIBILIDAD
        // ============================================

        // POST: api/atributo/comparar
        // Body: { "productoIds": [1, 2, 3] }
        [HttpPost("comparar")]
        public async Task<IActionResult> CompararProductos([FromBody] CompararProductosRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _atributoService.CompararProductosAsync(request.ProductoIds);
            return Ok(resultado);     
        }

        [HttpGet("asignaciones")]
        public async Task<IActionResult> GetAllAsignaciones()
        {
            var asignaciones = await _atributoService.GetAllAsignacionesAsync();
            var response = _mapper.Map<IEnumerable<AsignacionAtributoResponse>>(asignaciones);
            return Ok(response);
        }

        // GET: api/atributo/{atributoId}/productos?valor=X
        // Devuelve todos los productos que tienen ese atributo con ese valor
        [HttpGet("{atributoId}/productos")]
        public async Task<IActionResult> GetProductosConMismoValor(int atributoId, [FromQuery] string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return BadRequest(new { Error = "El valor es requerido" });

            var productos = await _atributoService.GetProductosConMismoAtributoValorAsync(atributoId, valor);
            var response = _mapper.Map<IEnumerable<DTOs.Responses.Productos.ProductoMiniResponse>>(productos);
            return Ok(response);
        }
    }
}