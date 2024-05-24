using System;
using System.IdentityModel.Tokens.Jwt; // for handling JWT (JSON Web Tokens) 
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens; // for handling JWT (JSON Web Tokens) 
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; //  for user management and authentication in ASP.NET Core applications
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; // for generating secure random numbers, important for creating secure tokens


namespace StudyBuddy.Controllers
{
    // declares AuthController as a controller with route Auth (inferred from the controller’s name 
    // "Controller") that can handle HTTP requests
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<IdentityUser> _passwordHasher;
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration,
                              PasswordHasher<IdentityUser> passwordHasher, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager; // manages users in the Identity system (creation, deletion, searching, etc.).
            _configuration = configuration; // to access application settings (like JWT settings)
            _passwordHasher = passwordHasher; // provides password hashing functionality to securely store user passwords
            _applicationDbContext = applicationDbContext; // for database access, particularly for managing refresh tokens
        }

        // can be deleted eventually :
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true  // confirming the email directly for testing or initial setup
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {              
                return Ok("User created successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // attempts to find a user by email, verify the password, generate a new refresh token
        // and a JWT access token, and then returns these tokens if the user is authenticated successfully.
        // attempts to find a user by email, verify the password, generate a new refresh token
        // and a JWT access token, and then returns these tokens if the user is authenticated successfully.

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User does not exist.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return BadRequest("Password is incorrect.");

            var refreshToken = new RefreshToken
            {
                Token = GenerateToken(),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                ReplacedByToken = null 
            };

            _applicationDbContext.RefreshTokens.Add(refreshToken);
            await _applicationDbContext.SaveChangesAsync();

            var accessToken = GenerateJwtToken(user);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = DateTime.UtcNow.AddMinutes(15),
                Role = role
            }) ;
        }

        // validates an existing refresh token, revokes it, and issues a new access and refresh token pair.
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var token = await _applicationDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.Token);
            /*
            if (token == null || token.Expires <= DateTime.UtcNow || token.Revoked != null)
            {
                return BadRequest("Invalid refresh token.");
            }
            */

            var user = await _userManager.FindByIdAsync(token.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            newRefreshToken.UserId = user.Id;

            // checking if the old refreshToken has a replacement and handle it properly
            if (!string.IsNullOrEmpty(token.ReplacedByToken))
            {
                // handling the logic (we can ignore for now)
            }

            // revoking old refreshToken and add the new one
            token.Revoked = DateTime.UtcNow;
            token.ReplacedByToken = newRefreshToken.Token;

            _applicationDbContext.RefreshTokens.Add(newRefreshToken);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresIn = DateTime.UtcNow.AddMinutes(15)  // expiration time --> JWT Token
            });
        }

        // HELPER METHODS

        // generates a JWT using the user’s details and application settings for token validation criteria
        // like issuer and audience
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("id", user.Id)  // claim (custom one) 
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),  // access token short lifespan
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // creates a new refresh token with a unique token string, expiry date, and creation date
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(7),  // refresh token long lifespan --> Refresh Token
                    Created = DateTime.UtcNow
                };
            }
        }

        private string GenerateToken()
        {
            var randomNumber = new byte[32]; // adjusting size as necessary
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber); // converting to base64 string
            }
        }
    }
}
