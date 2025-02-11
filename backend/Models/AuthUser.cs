namespace WyrdCodexAPI.Models
{
    public class AuthUser
    {
        public string? Username { get; set; }
        public required string Email { get; set; }

        public List<string>? Roles { get; set; }
    }
}
