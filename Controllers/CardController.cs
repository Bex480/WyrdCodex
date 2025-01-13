using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supabase.Interfaces;
using System.Security.Claims;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Models;
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
        private readonly WyrdCodexDbContext _context;

        public CardController(Supabase.Client supabase, CardService cardService, WyrdCodexDbContext context)
        {
            _supabase = supabase;
            _cardService = cardService;
            _context = context;
        }

        private async Task<Models.User?> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null) { return null; }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        [HttpPost]
        [Authorize(Roles = "RegisteredUser")]
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

        [HttpGet("shop")]
        public async Task<IActionResult> GetCardsUsingFiltersSHOP(string? cardName, string? type, string? faction)
        {
            var cards = await _cardService.GetCards(cardName, type, faction, null);

            return Ok(cards);
        }

        [HttpGet("collection")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> GetCardsUsingFiltersCOLLECTION(string? cardName, string? type, string? faction)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var collection = await _context.UserCards.Where(uc => uc.UserId == user.Id).Select(uc => uc.CardId).ToListAsync();

            var cards = await _cardService.GetCards(cardName, type, faction, collection);

            return Ok(cards);
        }

        [HttpPut]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> UpdateCard(int cardID, CardDTO cardDTO)
        {


            await _cardService.UpdateCard(cardID, cardDTO);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> DeleteCard(int cardID)
        {
            var card = await _cardService.GetCardByID(cardID);
            if (card == null) { return NotFound(); }

            await _cardService.DeleteCard(card);
            return Ok();
        }

        // Not fully implemented yet
        [HttpPost("buy")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> BuyCard(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            await _cardService.AddCardToCollection(user, cardID);

            return Ok();
        }

        [HttpPost("deck")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> CreateNewDeck(string deckName, IFormFile deckCoverImage)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            await _cardService.CreateDeck(user, deckName, deckCoverImage);

            return Created();
        }

        [HttpPost("add_to_deck")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> AddCardToDeck(int deckID, int cardID)
        { 
            await _cardService.AddCardToDeck(deckID, cardID);

            return Ok();
        }

        [HttpDelete("remove_from_deck")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> RemoveCardFromDeck(int deckID, int cardID)
        {
            await _cardService.RemoveCardFromDeck(deckID, cardID);

            return Ok();
        }

        [HttpGet("decks")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> GetDecks()
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var deck = await _cardService.GetDecks(user);

            return Ok(deck);
        }


        [HttpGet("deck")]
        public async Task<IActionResult> GetDeck(int deckID)
        {
            var deck = await _cardService.GetDeck(deckID);

            return Ok(deck);
        }


        [HttpDelete("deck")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> DeleteDeckWithID(int deckID)
        {
            await _cardService.DeleteDeck(deckID);

            return Ok();
        }
    }
}
