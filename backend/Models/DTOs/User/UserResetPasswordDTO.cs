namespace WyrdCodexAPI.Models.DTOs.User
{
    public class UserResetPasswordDTO
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
