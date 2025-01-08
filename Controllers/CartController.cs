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

        [HttpPost("add_card")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> AddToCart(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.AddCardToCart(cart.CartId, cardID);

            return Ok();
        }

        [HttpPut("change_card_quantity")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> ChangeQuantity(int cardID, int quantity)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.SetCardQuantityInCart(cart.CartId, cardID, quantity);

            return Ok();
        }

        [HttpDelete("remove_card")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> RemoveCard(int cardID)
        {
            var user = await GetCurrentUser();
            if (user == null) { return Unauthorized(); }

            var cart = await _shoppingService.GetOrCreateCart(user);

            await _shoppingService.RemoveCardFromCart(cart.CartId, cardID);

            return Ok();
        }

    }
}
