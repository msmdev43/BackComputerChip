namespace computerChip.DTOs.Responses.AdminDashboard
{
    public class AdminResponse
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
