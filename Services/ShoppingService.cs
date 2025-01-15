using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue.Mfa;
using System.Reflection.Metadata.Ecma335;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Migrations;
using WyrdCodexAPI.Models;
using WyrdCodexAPI.Models.DTOs;

namespace WyrdCodexAPI.Services
{
    public class ShoppingService
    {

        private readonly WyrdCodexDbContext _context;
        private readonly IEmailService _emailService;
        private readonly CardService _cardService;

        public ShoppingService(WyrdCodexDbContext context, IEmailService emailService, CardService cardService) 
        { 
            _context = context;
            _emailService = emailService;
            _cardService = cardService;
        }

        public async Task Checkout(User user)
        {
            var cart = await GetOrCreateCart(user);
            if (cart == null) { return; }

            float total = 0;
            List<ReceiptDTO> receiptDTOs = []; 

            var cards = cart.Cards;
            foreach (var card in cards)
            {
                total += card.Card.Price * card.Quantity;

                receiptDTOs.Add(
                    new ReceiptDTO()
                    {
                        CardName = card.Card.CardName,
                        Quantity = card.Quantity,
                        UnitPrice = card.Card.Price
                    });
            }

            await SendReceipt(user.Email, receiptDTOs, total);

            await ClearCart(cart.CartId);

            foreach (var card in cards)
            {
                for (var i = 0; i < card.Quantity; i++)
                {
                    await _cardService.AddCardToCollection(user, card.Card.Id);
                }
            }

        }

        public async Task SendReceipt(string email, List<ReceiptDTO> receipts, float total) 
        {
            var message = "<table style='width:100%; border-collapse: collapse;'>";

            message += "<tr style='text-align: left;'>";
            message += "<th style='padding: 8px;'>Card Name</th>";
            message += "<th style='padding: 8px;'>Unit Price</th>";
            message += "<th style='padding: 8px;'>Quantity</th>";
            message += "</tr>";

            foreach (var receipt in receipts)
            {
                message += "<tr>";
                message += $"<td style='padding: 8px;'>{receipt.CardName}</td>";
                message += $"<td style='padding: 8px;'>${receipt.UnitPrice}</td>";
                message += $"<td style='padding: 8px;'>X {receipt.Quantity}</td>";
                message += "</tr>";
            }

            message += "<tr style='font-weight: bold;'>";
            message += "<td style='padding: 8px;'>Total</td>";
            message += $"<td colspan='2' style='padding: 8px;'>${total}</td>";
            message += "</tr>";

            message += "</table>";

            await _emailService.SendEmail("otisthefatbear@gmail.com", "Receipt", message);
        }

        public async Task ClearCart(int CartId)
        { 
            var cart = await _context.Carts.FindAsync(CartId);
            if (cart == null) { return; }

            var entries = _context.CartCards.Where(cc => cc.CartId == CartId);

            _context.CartCards.RemoveRange(entries);

            await _context.SaveChangesAsync();
        }

        public async Task<CartDTO> GetOrCreateCart(User user)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(cart => cart.UserId == user.Id);

            if (cart == null)
            {
                var newCart = _context.Carts.Add(new Cart() { UserId = user.Id });
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
            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId && cc.CartId == CartId);

            if (entry != null) { entry.Quantity += 1; }

            if (entry == null) { _context.CartCards.Add(new CartCard() { CartId = CartId, CardId = CardId }); }

            await _context.SaveChangesAsync();
        }

        public async Task DecreaseQuantityOfCardInCart(int CartId, int CardId)
        {
            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId && cc.CartId == CartId);

            if (entry != null) { 
                if (entry.Quantity > 1) { 
                    entry.Quantity -= 1;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveCardFromCart(int CartId, int CardId)
        {
            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId && cc.CartId == CartId);

            if (entry == null) { return; }

            _context.CartCards.Remove(entry);

            await _context.SaveChangesAsync();
        }

        public async Task SetCardQuantityInCart(int CartId, int CardId, int Quantity)
        {
            if (Quantity <= 0) { return; }

            var entry = await _context.CartCards.FirstOrDefaultAsync(cc => cc.CardId == CardId && cc.CartId == CartId);

            if (entry == null) { return; }

            entry.Quantity = Quantity;

            await _context.SaveChangesAsync();
        }

        public async Task AddToSaveForLater(User user, int CardId, int Quantity)
        { 
            var existingCard = await _context.SavedForLater.FirstOrDefaultAsync(sfl => sfl.CardId == CardId && sfl.UserId == user.Id);
            if (existingCard != null)
            {
                existingCard.Quantity += Quantity;
                await _context.SaveChangesAsync();
                return;
            }

            _context.SavedForLater.Add(new SaveForLater() { UserId = user.Id, CardId = CardId, Quantity = Quantity });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromSaveForLater(User user, int CardId)
        {
            var entry = await _context.SavedForLater.FirstOrDefaultAsync(sfl => sfl.UserId == user.Id && sfl.CardId == CardId);
            if (entry == null) { return; }

            _context.SavedForLater.Remove(entry);

            await _context.SaveChangesAsync();
        }

        public async Task<List<CardWithQuantityDTO>> GetCardsSavedForLater(User user)
        {
            var savedCards = await _context.SavedForLater
                .Where(sfl => sfl.UserId == user.Id)
                .Select(sfl => new CardWithQuantityDTO() { Card = sfl.Card, Quantity = sfl.Quantity })
                .ToListAsync();
            if (savedCards == null) { return []; }

            return savedCards;
        }
    }
}
