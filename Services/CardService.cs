using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Migrations;
using WyrdCodexAPI.Models;
using WyrdCodexAPI.Models.DTOs.Card;
using static System.Net.Mime.MediaTypeNames;

namespace WyrdCodexAPI.Services
{
    public class CardService
    {
        private readonly Supabase.Client _supabase;
        private readonly WyrdCodexDbContext _context;

        public CardService(Supabase.Client supabase, WyrdCodexDbContext context) 
        {
            _supabase = supabase;
            _context = context;
        }

        public async Task<Card?> GetCardByID(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            return card;
        }

        public async Task<List<Card>> GetCardsByIDs(List<int> ids)
        {
            var cards = await _context.Cards
                                      .Where(c => ids.Contains(c.Id))
                                      .ToListAsync();

            return cards;
        }

        public async Task<List<Card>> GetAllCards()
        {
            var cards = await _context.Cards.ToListAsync();

            return cards;
        }

        public async Task<Card?> GetCardByCardName(string cardName)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.CardName == cardName);

            return card;
        }

        public async Task<List<Card>> GetCardsByCardNames(List<string> cardNames)
        {
            var cards = await _context.Cards
                                      .Where(c => cardNames.Contains(c.CardName))
                                      .ToListAsync();

            return cards;
        }

        public async Task InsertCard(CardDTO cardDTO)
        {
            using var memoryStream = new MemoryStream();
            await cardDTO.Image.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            await _supabase.Storage.From("cards").Upload(imageBytes, $"{cardDTO.Faction}/{cardDTO.Type}/{cardDTO.Image.FileName}");

            var imageUrl = _supabase.Storage.From("cards").GetPublicUrl($"{cardDTO.Faction}/{cardDTO.Type}/{cardDTO.Image.FileName}");

            var newCard = new Card
            {
                CardName = cardDTO.CardName,
                Type = cardDTO.Type,
                Faction = cardDTO.Faction,
                ImageUrl = imageUrl,
            };

            _context.Cards.Add(newCard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCard(Card card)
        {
            await _supabase.Storage.From("cards").Move($"{card.Faction}/{card.Type}/{Path.GetFileName(card.ImageUrl)}",
                                                       $"Archive/{card.Faction}/{card.Type}/{Path.GetFileName(card.ImageUrl)}");

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCard(int CardId, CardDTO updatedCard)
        { 
            var existingCard = await _context.Cards.FindAsync(CardId);
            if (existingCard == null) { return; }

            using var memoryStream = new MemoryStream();
            await updatedCard.Image.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            await _supabase.Storage.From("cards").Move($"{existingCard.Faction}/{existingCard.Type}/{Path.GetFileName(existingCard.ImageUrl)}",
                                                       $"Archive/{existingCard.Faction}/{existingCard.Type}/{Path.GetFileName(existingCard.ImageUrl)}");

            await _supabase.Storage.From("cards").Upload(imageBytes,$"{updatedCard.Faction}/{updatedCard.Type}/{updatedCard.Image.FileName}");

            var newImageUrl = _supabase.Storage.From("cards").GetPublicUrl($"{updatedCard.Faction}/{updatedCard.Type}/{updatedCard.Image.FileName}");

            existingCard.CardName = updatedCard.CardName;
            existingCard.Type = updatedCard.Type;
            existingCard.Faction = updatedCard.Faction;
            existingCard.ImageUrl = newImageUrl;

            _context.Cards.Update(existingCard);
            await _context.SaveChangesAsync();
        }

    }
}
