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

namespace WyrdCodexAPI.Services
{
    public class AuthService
    {
        private readonly WyrdCodexDbContext _context;

        public AuthService(WyrdCodexDbContext context)
        {
            _context = context;
        }

        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";

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

    }
}
