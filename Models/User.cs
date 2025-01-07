namespace WyrdCodexAPI.Models
{
    public class User
    {
        public int Id { get; set; } = 0;
        public required string Email { get; set; }
        public string UserName { get; set; } = string.Empty;
        public required string Password { get; set; }
        public required string Salt { get; set; }
        public bool TwoFactorEnabled { get; set; } = false;

        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<UserCard>? UserCards { get; set; }
        public ICollection<UserDeck>? UserDecks { get; set; }
    }
}
