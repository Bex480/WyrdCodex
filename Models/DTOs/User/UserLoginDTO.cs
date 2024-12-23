namespace WyrdCodexAPI.Models.DTOs.User
{
    public class UserLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
