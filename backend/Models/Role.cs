namespace WyrdCodexAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string RoleName { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
