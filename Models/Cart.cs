namespace WyrdCodexAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; }

        public ICollection<CartCard>? CartCards { get; set; }
    }
}
