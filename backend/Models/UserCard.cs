using Microsoft.EntityFrameworkCore;
using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WyrdCodexAPI.Models
{
    public class UserCard
    {
        [Key]
        public int Id { get; set; } = 0;

        public int UserId { get; set; }
        public User User { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
