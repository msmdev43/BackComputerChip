namespace computerChip.DTOs.Responses.Auth
{
    public class AdminLoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public AdminResponse Admin { get; set; } = new();
    }

    public class AdminResponse
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
