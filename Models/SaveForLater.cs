namespace WyrdCodexAPI.Models
{
    public class SaveForLater
    {

        public int UserId { get; set; }
        public User User { get; set; }


        public int CardId { get; set; }
        public Card Card { get; set; }


        public int Quantity { get; set; } = 1;

    }
}
