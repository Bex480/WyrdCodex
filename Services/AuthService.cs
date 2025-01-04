using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WyrdCodexAPI.Models;
using WyrdCodexAPI.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using WyrdCodexAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace WyrdCodexAPI.Services
{
    public class AuthService
    {
        private readonly WyrdCodexDbContext _context;
        private readonly IEmailService _emailService;

        public AuthService(WyrdCodexDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string ValidNums = "1234567890";

        public string GeneratePassword(int length)
        {
            char[] password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    password[i] = ValidChars[data[i] % ValidChars.Length];
                }
            }

            return new string(password);
        }

        public string GenerateCode(int length)
        {
            char[] code = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    code[i] = ValidNums[data[i] % ValidNums.Length];
                }
            }

            return new string(code);
        }

        public string GenerateToken(AuthUser authUser)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(authUser),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
                Audience = "WyrdCodex.com",
                Issuer = "WyrdCodex.com"
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(AuthUser authUser)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, authUser.Email));

            foreach (var role in authUser.Roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public (string hashedPassword, byte[] salt) HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA256,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));

            return (hashedPassword, salt);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {

            if (!await VerifyPassword(email, password))
            {
                return null;
            }

            var user = await _context.Users
                   .Include(u => u.UserRoles)
                   .ThenInclude(ur => ur.Role)
                   .FirstOrDefaultAsync(u => u.Email == email);


            return GenerateToken(new AuthUser
            {
                Username = user.UserName,
                Email = email,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            });
        }

        public async Task<bool> VerifyPassword(string email, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);


            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: password,
                                    salt: salt,
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 10000,
                                    numBytesRequested: 256 / 8));


            return hashedPassword == user.Password;
        }

        public async Task Send2FAcode(User user)
        {
            var code = GenerateCode(4);

            DateTimeOffset expiryDateTime = DateTimeOffset.UtcNow.AddMinutes(15);

            var entry = new TwoFactorModel()
            {
                UserID = user.Id,
                Code = code,
                Email = user.Email,
                ExpiryDateTime = expiryDateTime,
            };

            _context.TwoFactorCodes.Add(entry);

            await _context.SaveChangesAsync();

            await _emailService.SendEmail(user.Email, "WyrdCodex 2FA", $"Your 2FA code is: {code}");
        }

        public async Task<bool> Verify2FAcode([EmailAddress] string email, string code)
        {
            var entry = await _context.TwoFactorCodes.FirstOrDefaultAsync(c => c.Code == code);

            if (entry == null || entry.Email != email || DateTimeOffset.UtcNow > entry.ExpiryDateTime)
            {
                return false;
            }

            return true;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(AuthSettings.PrivateKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task AssignRefreshToken(int userId, string refreshToken) 
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserID == userId);

            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
                await _context.SaveChangesAsync();
            }

            var entry = new RefreshTokenModel
            {
                UserID = userId,
                RefreshToken = refreshToken,
                ExpiryDateTime = DateTimeOffset.UtcNow.AddDays(30)
            };

            _context.RefreshTokens.Add(entry);

            await _context.SaveChangesAsync();
        }
    }
}
