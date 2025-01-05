using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supabase.Interfaces;
using WyrdCodexAPI.Models.DTOs.Card;
using WyrdCodexAPI.Services;

namespace WyrdCodexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly Supabase.Client _supabase;
        private readonly CardService _cardService;

        public CardController(Supabase.Client supabase, CardService cardService)
        {
            _supabase = supabase;
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(CardDTO cardDTO)
        {
            await _cardService.InsertCard(cardDTO);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCard(int cardID)
        { 
            var card = await _cardService.GetCardByID(cardID);
            if (card == null) { return NotFound(); }

            return Ok(card);
        }

        // Temporary endpoint for testing
        [HttpGet("All")]
        public async Task<IActionResult> GetCards()
        {
            var cards = await _cardService.GetAllCards();

            return Ok(cards);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCard(int cardID, CardDTO cardDTO)
        { 
            await _cardService.UpdateCard(cardID, cardDTO);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCard(int cardID)
        {
            var card = await _cardService.GetCardByID(cardID);
            if (card == null) { return NotFound(); }

            await _cardService.DeleteCard(card);
            return Ok();
        }

    }
}
