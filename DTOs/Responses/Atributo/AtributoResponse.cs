// DTOs/Responses/Atributo/AtributoResponse.cs
namespace computerChip.DTOs.Responses.Atributo
{
    public class AtributoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int ProductosAsociados { get; set; }
    }
}