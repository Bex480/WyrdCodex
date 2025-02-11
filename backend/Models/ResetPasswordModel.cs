using System.ComponentModel.DataAnnotations;

namespace WyrdCodexAPI.Models
{
    public class ResetPasswordModel
    {

        public int Id { get; set; } = 0;
        [Required]
        public int UserID { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public DateTimeOffset ExpiryDateTime { get; set; }

    }
}
