namespace WyrdCodexAPI.Models
{
    public class Deck
    {

        public int Id { get; set; }
        public required string DeckName { get; set; }
        public string ImageUrl { get; set; } = "https://zdmrsbsjmojecdeahdjp.supabase.co/storage/v1/object/public/deck_covers/public/default_cover.png?t=2025-01-07T16%3A31%3A59.637Z";

        public ICollection<UserDeck>? UserDecks { get; set; }
        public ICollection<DeckCard>? DeckCards { get; set; }

    }
}
