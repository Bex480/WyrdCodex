namespace WyrdCodexAPI.Models.DTOs.User
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
