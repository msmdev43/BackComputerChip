// DTOs/Requests/PushToken/PushTokenRegisterRequest.cs
namespace computerChip.DTOs.Requests.PushToken
{
    public class PushTokenRegisterRequest
    {
        public string Token { get; set; } = string.Empty;
        public string Dispositivo { get; set; } = string.Empty; // "android", "ios", "web"
    }

    public class PushTokenSendRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public object? Data { get; set; }
    }
}