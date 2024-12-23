namespace WyrdCodexAPI.Models.DTOs.User
{
    public class UserRegisterDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string UserName { get; set; } = string.Empty;

    }
}
