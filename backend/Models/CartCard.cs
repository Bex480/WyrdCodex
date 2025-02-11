namespace WyrdCodexAPI.Models
{
    public class CartCard
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        
        public int CardId { get; set; }
        public Card Card { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
