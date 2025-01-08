using Microsoft.EntityFrameworkCore;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Models;
using WyrdCodexAPI.Models.DTOs;

namespace WyrdCodexAPI.Services
{
    public class ShoppingService
    {

        private readonly WyrdCodexDbContext _context;

        public ShoppingService(WyrdCodexDbContext context) 
        { 
            _context = context;
        }

        public async Task<CartDTO> GetOrCreateCart(User user)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(cart => cart.UserId == user.Id && cart.IsActive);

            if (cart == null)
            {
                var newCart = _context.Carts.Add(new Cart() { UserId = user.Id });
                newCart.Entity.IsActive = true;
                await _context.SaveChangesAsync();

                return new CartDTO() { CartId = newCart.Entity.Id, Cards = [] };
            }

            var cards = await _context.CartCards
                                .Where(cc => cc.CartId == cart.Id)
                                .Select(
                                    cc => 
                                    new CardWithQuantityDTO{ 
                                        Card = cc.Card,
                                        Quantity = cc.Quantity
                                    }
                                )
                                .ToListAsync();

            return new CartDTO() { CartId = cart.Id, Cards = cards };
        }

        public async Task AddCardToCart(int CartId, int CardId)
        {
            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId);

            if (entry != null) { entry.Quantity += 1; }

            if (entry == null) { _context.CartCards.Add(new CartCard() { CartId = CartId, CardId = CardId }); }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCardFromCart(int CartId, int CardId)
        {
            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId);

            if (entry == null) { return; }

            _context.CartCards.Remove(entry);

            await _context.SaveChangesAsync();
        }

        public async Task SetCardQuantityInCart(int CartId, int CardId, int Quantity)
        {
            if (Quantity <= 0) { return; }

            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId);

            if (entry == null) { return; }

            entry.Quantity = Quantity;

            await _context.SaveChangesAsync();
        }
    }
}
