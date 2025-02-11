namespace WyrdCodexAPI.Models.DTOs
{
    public class ReceiptDTO
    {
        public required string CardName { get; set; }
        public int Quantity { get; set; } = 1;
        public float UnitPrice { get; set; }
    }
}
