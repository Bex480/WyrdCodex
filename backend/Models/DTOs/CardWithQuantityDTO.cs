namespace WyrdCodexAPI.Models.DTOs
{
    public class CardWithQuantityDTO
    {
        public required Models.Card Card { get; set; }

        public int Quantity { get; set; }

    }
}
