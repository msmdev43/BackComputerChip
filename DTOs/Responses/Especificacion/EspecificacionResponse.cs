// DTOs/Responses/Especificacion/EspecificacionResponse.cs
namespace computerChip.DTOs.Responses.Especificacion
{
    public class EspecificacionResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int ProductosAsociados { get; set; }
    }
}