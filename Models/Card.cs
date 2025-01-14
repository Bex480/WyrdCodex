namespace WyrdCodexAPI.Models
{
    public class Card
    {
        public int Id { get; set; } = 0;
        public required string CardName { get; set; }
        public required string Type { get; set; }
        public required string Faction { get; set; }
        public required string ImageUrl { get; set; }

        public ICollection<UserCard>? UserCards { get; set; }
        public ICollection<DeckCard>? DeckCards { get; set; }
        public ICollection<CartCard>? CartCards { get; set; }
        public ICollection<SaveForLater>? SavedForLater { get; set; }

    }
}
