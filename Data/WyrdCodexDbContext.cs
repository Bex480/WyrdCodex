using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WyrdCodexAPI.Models;

namespace WyrdCodexAPI.Data
{
    public class WyrdCodexDbContext : DbContext
    {
        public WyrdCodexDbContext(DbContextOptions<WyrdCodexDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ResetPasswordModel> ResetPasswordRequests { get; set; }
        public DbSet<TwoFactorModel> TwoFactorCodes { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
        public DbSet<UserDeck> UserDecks { get; set; }
        public DbSet<DeckCard> DeckCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "RegisteredUser" }
            );

            modelBuilder.Entity<UserCard>()
                .HasKey(uc => new { uc.UserId, uc.CardId });

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCards)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.Card)
                .WithMany(c => c.UserCards)
                .HasForeignKey(uc => uc.CardId);

            modelBuilder.Entity<UserDeck>()
                .HasKey(ud => new { ud.UserId, ud.DeckId });

            modelBuilder.Entity<UserDeck>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDecks)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDeck>()
                .HasOne(ud => ud.Deck)
                .WithMany(d => d.UserDecks)
                .HasForeignKey(ud => ud.DeckId);

            modelBuilder.Entity<DeckCard>()
                .HasKey(dc => new { dc.DeckId, dc.CardId });

            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Deck)
                .WithMany(d => d.DeckCards)
                .HasForeignKey(dc => dc.DeckId);

            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Card)
                .WithMany(c => c.DeckCards)
                .HasForeignKey(dc => dc.CardId);
        }
    }
}
