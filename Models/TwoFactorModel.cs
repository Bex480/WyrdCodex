using System.ComponentModel.DataAnnotations;

namespace WyrdCodexAPI.Models
{
    public class TwoFactorModel
    {
        public int Id { get; set; } = 0;
        public int UserID { get; set; }
        public required string Code { get; set; }
        public required string Email { get; set; }

        public DateTimeOffset ExpiryDateTime { get; set; }
    }
}
