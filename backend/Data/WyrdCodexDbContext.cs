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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartCard> CartCards { get; set; }
        public DbSet<SaveForLater> SavedForLater { get; set; }

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
                .HasKey(uc => new { uc.Id });

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCards)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.Card)
                .WithMany(c => c.UserCards)
                .HasForeignKey(uc => uc.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDeck>()
                .HasKey(ud => new { ud.UserId, ud.DeckId });

            modelBuilder.Entity<UserDeck>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDecks)
                .HasForeignKey(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDeck>()
                .HasOne(ud => ud.Deck)
                .WithMany(d => d.UserDecks)
                .HasForeignKey(ud => ud.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeckCard>()
                .HasKey(dc => new { dc.Id });

            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Deck)
                .WithMany(d => d.DeckCards)
                .HasForeignKey(dc => dc.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Card)
                .WithMany(c => c.DeckCards)
                .HasForeignKey(dc => dc.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartCard>()
                .HasKey(cc => new { cc.CartId, cc.CardId });

            modelBuilder.Entity<CartCard>()
                .HasOne(cc => cc.Cart)
                .WithMany(c => c.CartCards)
                .HasForeignKey(cc => cc.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartCard>()
                .HasOne(cc => cc.Card)
                .WithMany(c => c.CartCards)
                .HasForeignKey(cc => cc.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaveForLater>()
                .HasKey(sfl => new { sfl.UserId, sfl.CardId });

            modelBuilder.Entity<SaveForLater>()
                .HasOne(sfl => sfl.User)
                .WithMany(u => u.SavedForLater)
                .HasForeignKey(sfl => sfl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaveForLater>()
                .HasOne(sfl => sfl.Card)
                .WithMany(c => c.SavedForLater)
                .HasForeignKey(sfl => sfl.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardName);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.Type);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.Faction);

        }
    }
}
    