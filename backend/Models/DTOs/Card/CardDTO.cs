namespace WyrdCodexAPI.Models.DTOs.Card
{
    public class CardDTO
    {
        public required string CardName { get; set; }
        public required string Type { get; set; }
        public required string Faction { get; set; }
        public IFormFile? Image { get; set; }
    }
}
