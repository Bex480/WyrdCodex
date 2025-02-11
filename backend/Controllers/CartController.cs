using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Migrations;
using WyrdCodexAPI.Services;

namespace WyrdCodexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ShoppingService _shoppingService;
        private readonly WyrdCodexDbContext _context;

        public CartController(ShoppingService shoppingService, WyrdCodexDbContext context) 
        { 
            _shoppingService = shoppingService;
            _context = context;
        }

        private async Task<Models.User?> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null) { return null; }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        [HttpGet]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> GetCart()
        { 
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);
         
            return Ok(cart.Cards);
        }

        [HttpPost("card/add")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> AddToCart(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.AddCardToCart(cart.CartId, cardID);

            return Ok();
        }

        [HttpPut("card/quantity/decrease")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> DecreaseQuantity(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.DecreaseQuantityOfCardInCart(cart.CartId, cardID);

            return Ok();
        }

        [HttpPut("card/quantity/set")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> ChangeQuantity(int cardID, int quantity)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.SetCardQuantityInCart(cart.CartId, cardID, quantity);

            return Ok();
        }

        [HttpDelete("card/remove")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> RemoveCard(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.RemoveCardFromCart(cart.CartId, cardID);

            return Ok();
        }

        [HttpPost("card/save_for_later/add")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> AddToSaveForLater(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            var card = cart.Cards.FirstOrDefault(c => c.Card.Id == cardID);
            if (card == null) { return Unauthorized(); }

            if (card != null) 
            { 
                await _shoppingService.AddToSaveForLater(user, cardID, card.Quantity);
                await _shoppingService.RemoveCardFromCart(cart.CartId, cardID);
            }

            return Ok();
        }

        [HttpDelete("card/save_for_later/remove")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> RemoveFromSaveForLater(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            await _shoppingService.RemoveFromSaveForLater(user, cardID);

            return Ok();
        }

        [HttpGet("saved_for_later")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> GetCardsSavedForLater()
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cards = await _shoppingService.GetCardsSavedForLater(user);

            return Ok(cards);
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> Checkout()
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            await _shoppingService.Checkout(user);

            return Ok();
        }

    }
}
