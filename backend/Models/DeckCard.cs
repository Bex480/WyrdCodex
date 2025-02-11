﻿using System.ComponentModel.DataAnnotations;

namespace WyrdCodexAPI.Models
{
    public class DeckCard
    {
        [Key]
        public int Id { get; set; }


        public int DeckId { get; set; }
        public Deck Deck { get; set; }


        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
