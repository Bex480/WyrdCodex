using WyrdCodexAPI.Migrations;
using WyrdCodexAPI.Models;

namespace WyrdCodexAPI.Models.DTOs.Card
{
    public class DeckDTO
    {
        public required Deck Deck { get; set; }
        public required List<Models.Card> Cards { get; set; }
    }
}
