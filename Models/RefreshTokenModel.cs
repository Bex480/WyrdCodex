namespace WyrdCodexAPI.Models
{
    public class RefreshTokenModel
    {
        public int Id { get; set; } = 0;
        public int UserID { get; set; }
        public required string RefreshToken { get; set; }
    
        public DateTimeOffset ExpiryDateTime { get; set; }
    }
}
