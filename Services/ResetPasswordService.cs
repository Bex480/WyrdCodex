using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Helpers;
using WyrdCodexAPI.Models;

namespace WyrdCodexAPI.Services
{
    public class ResetPasswordService
    {
        private readonly WyrdCodexDbContext _context;
        private readonly AuthService _authService;
        private readonly IEmailService _emailService;

        public ResetPasswordService(WyrdCodexDbContext context, AuthService authService, IEmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task SendPasswordResetLink(User user)
        {
            string token = _authService.GeneratePassword(64);
            var (hashedToken, salt) = _authService.HashPassword(token);
            DateTimeOffset expiryDateTime = DateTimeOffset.UtcNow.AddMinutes(15);

            var entry = new ResetPasswordModel()
            {
                UserID = user.Id,
                Token = hashedToken,
                Salt = Convert.ToBase64String(salt),
                ExpiryDateTime = expiryDateTime,
            };

            _context.ResetPasswordRequests.Add(entry);

            await _context.SaveChangesAsync();

            string baseUrl = FrontendSettings.Host;
            string resetPasswordUrl = $"{baseUrl}/auth/reset-password?token={token}";

            await _emailService.SendEmail(user.Email, "WyrdCodex Password Reset", "Click here to reset password: " +  resetPasswordUrl);
        }

    }
}
