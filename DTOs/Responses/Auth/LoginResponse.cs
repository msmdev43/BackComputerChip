namespace computerChip.DTOs.Responses.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public UsuarioLoginResponse Usuario { get; set; } = new();
    }

    public class UsuarioLoginResponse
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public bool IsGoogleUser { get; set; }
    }
}