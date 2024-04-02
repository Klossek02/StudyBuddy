using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StudyBuddy.Models;
using StudyBuddy.DTO;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<UserModel> _passwordHasher;
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            PasswordHasher<UserModel> passwordHasher, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _applicationDbContext = applicationDbContext;   
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //var user = await _userManager.FindByEmailAsync(model.Email); // not working

            // checking Admins, Students and Tutors table (I know not the best way to do this)
            var user = await _applicationDbContext.Admins
                .Select(admin => new UserModel
                {
                    Email = admin.Email,
                    PasswordHash = admin.PasswordHash
                })
                .FirstOrDefaultAsync(s => s.Email == model.Email);
            if (user == null)
            {
                user = await _applicationDbContext.Students
                .Select(student => new UserModel
                {
                    Email = student.Email,
                    PasswordHash = student.PasswordHash
                })
                .FirstOrDefaultAsync(s => s.Email == model.Email);

                if (user == null)
                {
                    user = await _applicationDbContext.Tutors.Select(t => new UserModel
                    {
                        Email = t.Email,
                        PasswordHash = t.PasswordHash
                    }).FirstOrDefaultAsync(s => s.Email == model.Email);
                }
            }

            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            // Verify the password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                // Password is correct, perform login
                //var token = GenerateJwtToken(user);

                //return Ok(new TokenModel { Token = token, Expiration = DateTime.UtcNow.AddHours(2) });
                return Ok("Login successful");
            }
            else
            {
                return BadRequest("Invalid email or password.");
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]); // our secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
