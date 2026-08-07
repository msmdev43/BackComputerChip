// DTOs/Responses/PushToken/PushTokenResponse.cs
namespace computerChip.DTOs.Responses.PushToken
{
    public class PushTokenResponse
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Dispositivo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? AdminNombre { get; set; }
    }

    public class PushTokenSendResponse
    {
        public int Enviados { get; set; }
        public int Fallidos { get; set; }
        public List<string>? Errores { get; set; }
    }
}