namespace WyrdCodexAPI.Models
{
    public class Card
    {
        public int Id { get; set; } = 0;
        public required string CardName { get; set; }
        public required string Type { get; set; }
        public required string Faction { get; set; }
        public required string ImageUrl { get; set; }
    }
}
