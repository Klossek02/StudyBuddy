using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;


namespace StudyBuddy
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, ApplicationDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }

        // allowing user to register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        // allowing user to login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);

                return Ok(new TokenModel { Token = token, Expiration = DateTime.UtcNow.AddHours(2) });
            }

            return Unauthorized();
        }

        // allowing user to change their password (only for authenticated users)
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("Error. User is not authenticated.");

            var changePassword = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePassword.Succeeded)
            {
                return BadRequest(changePassword.Errors);
            }

            return Ok("Password changed successfully!");
        }

        // allowing user to reset password 
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid User. Please choose User once again.");

            var resetPassword = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!resetPassword.Succeeded)
            {
                return BadRequest(resetPassword.Errors);
            }

            return Ok("Password has been reset successfully!");
        }

        // endpoint as if user forgets password 
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Ok("In case email is registered, you will receive a password reset email.");
            }

            // password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // sending token to user (via email)
            var resetLink = Url.Action("ResetPassword", "Auth", new { token = token, email = user.Email }, Request.Scheme);
            await _emailService.SendAsync(user.Email, "Reset Your Password", $"Please reset your password by clicking here: {resetLink}");

            return Ok();
        }

        // endpoint for logging out
        [HttpPost("log-out")]
        public async Task<IActionResult> Logout()
        {
            return Ok("You have been logged out.");
        }

        // endpoint for verifying email
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid Email");
            }
                
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email verified successfully!");
            }
            else
            {
                return BadRequest("Failed to verify email");
            }
        }

        // endpoint for resend verification email
        [HttpPost("resend-verification-email")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                return Ok("In case your email is registered and not yet verified, you will get another verification email.");
            }

            // email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var verificationLink = Url.Action("VerifyEmail", "Auth", new { token = token, email = user.Email }, Request.Scheme);

            // sending verification email
            await _emailService.SendAsync(user.Email, "Verify Your Email", $"Please verify your email by clicking here: {verificationLink}");

            return Ok();
        }


        // allowing particular user - Student to register
        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student"); // give Student role

                var student = new Student  // we don't store password here - it's done by Identity
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmailVerified = false, // updated after email confirmation
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Students.Add(student); 
                await _context.SaveChangesAsync(); // saving Student to database

                return Ok(); 
            }

            return BadRequest(result.Errors);
        }


        // allowing particular user - Tutor to register
        [HttpPost("register-tutor")]
        public async Task<IActionResult> RegisterTutor([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Tutor"); // give Tutor role

                // we don't store password here - it's done by Identity
                var tutor = new Tutor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmailVerified = false, // updated after email confirmation
                    ExpertiseArea = model.ExpertiseArea,
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Tutors.Add(tutor);
                await _context.SaveChangesAsync(); // saving Tutor to database

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        // allowing particular user - Admin to register (done in a different way than Student and Tutor
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin"); // give Admin role

                var admin = new Admin
                {
                    DisplayName = model.FirstName + " " + model.LastName,
                    Username = model.UserName,
                    Email = model.Email,
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync(); // saving Admin to database

                return Ok();

            }

            return BadRequest(result.Errors);
        }

        // function for generating JWT token
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

    // model for token
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

    // model for registration
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string ExpertiseArea { get; set; }

    }

    // model for login
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // model for chaning password
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set;}
        public string NewPassword { get; set; }
    } 

    // model for resetting password
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    // model in case password is forgotten
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    // model for resending verification email
    public class ResendVerificationEmailModel
    {
        public string Email { get; set; }
    }
}
