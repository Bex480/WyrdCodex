namespace WyrdCodexAPI.Models.DTOs
{
    public class CartDTO
    {
        public required int CartId { get; set; }
        public required List<CardWithQuantityDTO> Cards { get; set; }
    }
}
