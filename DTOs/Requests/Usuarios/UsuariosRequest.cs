// DTOs/Requests/Usuario/UsuarioRegisterRequest.cs
namespace computerChip.DTOs.Requests.Usuario
{
    public class UsuarioRegisterRequest
    {
        public string? NombreCompleto { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Pais { get; set; }
        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Celular { get; set; }
    }

    public class UsuarioLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioUpdateRequest
    {
        public string? NombreCompleto { get; set; }
        public string? Pais { get; set; }
        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Celular { get; set; }
    }

    public class UsuarioChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UsuarioGoogleLoginRequest
    {
        public string GoogleSub { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? RefreshToken { get; set; }
    }
}