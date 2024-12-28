using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WyrdCodexAPI.Data;
using WyrdCodexAPI.Models;
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
            var token = await _authService.LoginAsync(userLoginDTO.Email, userLoginDTO.Password);

            if (token == null)
            {
                return Unauthorized("Wrong Email or Password!");
            }

            return Ok(token);
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
                new UserDTO(){ 
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
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest();
            }

            await _resetPasswordService.SendPasswordResetLink(user);

            return Accepted();
        }
    }
}
