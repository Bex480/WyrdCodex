﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Models;
using WyrdCodexAPI.Models.DTOs;
using WyrdCodexAPI.Models.DTOs.User;
using WyrdCodexAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WyrdCodexAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly WyrdCodexDbContext _context;
		private readonly AuthService _authService;
		private readonly IEmailService _emailService;
		private readonly ResetPasswordService _resetPasswordService;

		public UserController(WyrdCodexDbContext context, AuthService authService, IEmailService emailService, ResetPasswordService resetPasswordService)
		{
			_context = context;
			_authService = authService;
			_emailService = emailService;
			_resetPasswordService = resetPasswordService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);
			if (user == null)
			{
				return Unauthorized();
			}

			var token = await _authService.LoginAsync(userLoginDTO.Email, userLoginDTO.Password);

			if (token == null)
			{
				return Unauthorized();
			}

			if (user.TwoFactorEnabled)
			{
				await _authService.Send2FAcode(user);
				return Accepted();
			}

			var refreshToken = _authService.GenerateRefreshToken();
			await _authService.AssignRefreshToken(user.Id, refreshToken);

			return Ok(new { token, refreshToken });
		}

		[HttpPost("login_2FA")]
		public async Task<IActionResult> Login2FA([EmailAddress] string email, string code)
		{
			if (await _authService.Verify2FAcode(email, code))
			{
				var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == email);

				if (user == null)
				{
					return Unauthorized();
				}

				var token = _authService.GenerateToken(new AuthUser
				{
					Username = user.UserName,
					Email = email,
					Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
				});

				var refreshToken = _authService.GenerateRefreshToken();
                await _authService.AssignRefreshToken(user.Id, refreshToken);

                return Ok(new { token, refreshToken });
			}
			return Unauthorized();
		}

		// GET: api/<UserController>
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			return Ok(await _context.Users.ToListAsync());
		}


		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(
				new UserDTO() {
					Email = user.Email,
					UserName = user.UserName
				});
		}

		// POST api/<UserController>
		[HttpPost("register")]
		public async Task<IActionResult> CreateNewUser([FromBody] UserRegisterDTO newUser)
		{
			var emailInUse = await _context.Users.AnyAsync(u => u.Email == newUser.Email);

			if (emailInUse)
			{
				return Conflict(new { message = "A user with this E-mail already exists!" });
			}

			var (hashedPassword, salt) = _authService.HashPassword(newUser.Password);

			_context.Users.Add(
				new User()
				{
					Email = newUser.Email,
					UserName = newUser.UserName,
					Password = hashedPassword,
					Salt = Convert.ToBase64String(salt)
				});

			await _context.SaveChangesAsync();

			var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

			var registeredRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "RegisteredUser");

			_context.UserRoles.Add(new UserRole
			{
				UserId = createdUser.Id,
				RoleId = registeredRole.Id
			});

			await _context.SaveChangesAsync();

			return Created();
		}

		// PUT api/<UserController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO updatedUser)
		{
			var existingUser = await _context.Users.FindAsync(id);

			if (existingUser == null)
			{
				return NotFound();
			}

			if (updatedUser.Email != existingUser.Email)
			{
				var emailInUse = await _context.Users.AnyAsync(u => u.Email == updatedUser.Email);

				if (emailInUse)
				{
					return Conflict(new { message = "A user with this E-mail already exists!" });
				}
			}

			existingUser.Email = updatedUser.Email;
			existingUser.UserName = updatedUser.UserName;

			await _context.SaveChangesAsync();

			return Ok();
		}

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		[Authorize(Roles = "RegisteredUser")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var existingUser = await _context.Users.FindAsync(id);

			if (existingUser == null)
			{
				return NotFound();
			}

			_context.Remove(existingUser);
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("email")]
		public async Task<IActionResult> SendDirectMail(string receptor, string subject, string body)
		{
			await _emailService.SendEmail(receptor, subject, body);
			return Ok();
		}

		[HttpPost("request_password_reset")]
		public async Task<IActionResult> RequestPasswordReset([EmailAddress] string email)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest("INVALID EMAIL FORMAT!!!!!!!!!!!!");
			}

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				return BadRequest();
			}

			await _resetPasswordService.SendPasswordResetLink(user);

			return Accepted();
		}

		[HttpPut("reset_password")]
		public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDTO userResetPassword)
		{
			var resetRequest = await _context.ResetPasswordRequests.FirstOrDefaultAsync(r => r.Token == userResetPassword.Token);

			if (resetRequest == null)
			{
				return BadRequest("Invalid Token");
			}

			if (DateTimeOffset.UtcNow > resetRequest.ExpiryDateTime)
			{
				return Unauthorized("Your token has expired please request a new one.");
			}

			var user = await _context.Users.FindAsync(resetRequest.UserID);

			if (user == null)
			{
				return NotFound();
			}

			var (hashedPassword, salt) = _authService.HashPassword(userResetPassword.NewPassword);

			user.Password = hashedPassword;
			user.Salt = Convert.ToBase64String(salt);

			await _context.SaveChangesAsync();

			return Ok();
		}


        [HttpGet("profile")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> GetUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
         
            if (email == null)
            {
                return Unauthorized();
            }

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				return NotFound();
			}

			var userDTO = new UserDTO
			{
				Email = email,
				UserName = user.UserName,
				Is2FAenabled = user.TwoFactorEnabled
			};
            
            return Ok(userDTO);
        }

        [HttpPut("twofactor")]
        [Authorize(Roles = "RegisteredUser")]
        public async Task<IActionResult> Toggle2FA()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound();
            }

			user.TwoFactorEnabled = !user.TwoFactorEnabled;

			await _context.SaveChangesAsync();

            return Ok();
        }

		[HttpPost("refresh_token")]
		public async Task<IActionResult> RefreshToken([FromBody] TokensDTO tokensDTO)
		{ 
			var principal = _authService.GetPrincipalFromExpiredToken(tokensDTO.AccessToken);

			var email = principal.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null) { return Unauthorized("email"); }

            var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) { return Unauthorized("user"); }

            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserID == user.Id);
            if (existingToken == null) { return Unauthorized("token"); }

            if (existingToken.RefreshToken != tokensDTO.RefreshToken) { return Unauthorized("reftoken"); }

			if (DateTimeOffset.UtcNow > existingToken.ExpiryDateTime) { return Unauthorized("time"); }

            var newAccessToken = _authService.GenerateToken(new AuthUser
            {
                Username = user.UserName,
                Email = email,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            });

			return Ok(new { token = newAccessToken });
		}

    }
}
