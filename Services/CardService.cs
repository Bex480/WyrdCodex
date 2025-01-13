using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Supabase.Postgrest.Attributes;
using System.Text.Json.Nodes;
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
        private readonly AuthService _authService;

        public CardService(Supabase.Client supabase, WyrdCodexDbContext context, AuthService authService)
        {
            _supabase = supabase;
            _context = context;
            _authService = authService;
        }

        public async Task<Card?> GetCardByID(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            return card;
        }

        public async Task<List<Card>> GetCardsByIDs(List<int> ids)
        {
            var cards = await _context.Cards.Join(ids,
                                             c => c.Id,
                                             id => id,
                                             (c, id) => c)
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

        public async Task<List<Card>> GetCards(string? cardName, string? type, string? faction, List<int>? collection)
        {
            List<Card> allCards = [];

            if (collection == null) { allCards = await GetAllCards(); }
            else { allCards = await GetCardsByIDs(collection); }

            var filteredCards = allCards.AsQueryable();

            if (!string.IsNullOrEmpty(cardName)) { filteredCards = filteredCards.Where(c => c.CardName.Contains(cardName, StringComparison.OrdinalIgnoreCase)); }

            if (!string.IsNullOrEmpty(type)) { filteredCards = filteredCards.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase)); }

            if (!string.IsNullOrEmpty(faction)) { filteredCards = filteredCards.Where(c => c.Faction.Equals(faction, StringComparison.OrdinalIgnoreCase)); }

            return filteredCards.ToList();
        }

        public async Task CreateDeck(User user, string deckName, IFormFile deckCoverImage)
        {
            using var memoryStream = new MemoryStream();
            await deckCoverImage.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            var cyph = _authService.GeneratePassword(5);

            await _supabase.Storage.From("deck_covers").Upload(imageBytes, $"public/{cyph + deckCoverImage.FileName}");

            var imageUrl = _supabase.Storage.From("deck_covers").GetPublicUrl($"public/{cyph + deckCoverImage.FileName}");

            var newDeck = new Deck
            {
                DeckName = deckName,
                ImageUrl = imageUrl
            };

            var addedDeck = _context.Decks.Add(newDeck);
            await _context.SaveChangesAsync();

            _context.UserDecks.Add(new UserDeck
            {
                UserId = user.Id,
                DeckId = addedDeck.Entity.Id
            });

            await _context.SaveChangesAsync();
        }

        public async Task InsertCard(CardDTO cardDTO)
        {
            using var memoryStream = new MemoryStream();
            await cardDTO.Image.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            var cyph = _authService.GeneratePassword(5);

            await _supabase.Storage.From("cards").Upload(imageBytes, $"{cardDTO.Faction}/{cardDTO.Type}/{cyph + cardDTO.Image.FileName}");

            var imageUrl = _supabase.Storage.From("cards").GetPublicUrl($"{cardDTO.Faction}/{cardDTO.Type}/{cyph + cardDTO.Image.FileName}");

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

            var imageUrl = existingCard.ImageUrl;

            if (updatedCard.Image != null)
            {
                using var memoryStream = new MemoryStream();
                await updatedCard.Image.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                await _supabase.Storage.From("cards").Move($"{existingCard.Faction}/{existingCard.Type}/{Path.GetFileName(existingCard.ImageUrl)}",
                                                           $"Archive/{existingCard.Faction}/{existingCard.Type}/{Path.GetFileName(existingCard.ImageUrl)}");

                var cyph = _authService.GeneratePassword(5);

                await _supabase.Storage.From("cards").Upload(imageBytes, $"{updatedCard.Faction}/{updatedCard.Type}/{cyph + updatedCard.Image.FileName}");

                imageUrl = _supabase.Storage.From("cards").GetPublicUrl($"{updatedCard.Faction}/{updatedCard.Type}/{cyph + updatedCard.Image.FileName}");
            }

            existingCard.CardName = updatedCard.CardName;
            existingCard.Type = updatedCard.Type;
            existingCard.Faction = updatedCard.Faction;
            existingCard.ImageUrl = imageUrl;

            _context.Cards.Update(existingCard);
            await _context.SaveChangesAsync();
        }

        public async Task AddCardToCollection(User user, int CardId)
        {
            _context.UserCards.Add(new UserCard() { UserId = user.Id, CardId = CardId });

            await _context.SaveChangesAsync();
        }

        public async Task AddCardToDeck(int DeckId, int CardId)
        {
            _context.DeckCards.Add(new DeckCard() { DeckId = DeckId, CardId = CardId });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCardFromDeck(int DeckId, int CardId)
        {
            var entry = await _context.DeckCards.FirstOrDefaultAsync(dc => dc.DeckId == DeckId && dc.CardId == CardId);
            if (entry != null)
            {
                _context.DeckCards.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Deck>> GetDecks(User user)
        {
            var decks = await _context.UserDecks.Where(ud => ud.UserId == user.Id).Select(ud => ud.Deck).ToListAsync();

            return decks;
        }

        public async Task<DeckDTO?> GetDeck(int DeckId)
        {
            var deck = await _context.Decks.FindAsync(DeckId);
            if (deck == null) { return null; }

            var cards = await _context.DeckCards.Where(dc => dc.DeckId == DeckId).Select(dc => dc.Card).ToListAsync();

            return new DeckDTO() { Deck = deck, Cards = cards };
        }

        public async Task DeleteDeck(int DeckId)
        {
            var deck = await _context.Decks.FindAsync(DeckId);
            if (deck == null) { return; }

            _context.Decks.Remove(deck);

            await _context.SaveChangesAsync();
        }
    }
}
