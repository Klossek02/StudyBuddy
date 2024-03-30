using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Models;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // for handling passwords securely

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class AdminCreateModel
        {
            [Required]
            [StringLength(100)]
            public string DisplayName { get; set; }

            [Required]
            [StringLength(50)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; } // for simplicity, assuming plain text to be hashed in controller/service
        }

        public class AdminDTO
        {
            public int AdminId { get; set; }
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public DateTime RegistrationDate { get; set; }
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDTO>>> GetAdmins()
        {
            var admins = await _context.Admins
                .Select(a => new AdminDTO
                {
                    AdminId = a.AdminId,
                    DisplayName = a.DisplayName,
                    Username = a.Username,
                    Email = a.Email,
                    RegistrationDate = a.RegistrationDate
                })
                .ToListAsync();

            return Ok(admins);
        }

        // POST: api/Admins
        [HttpPost]
        public async Task<ActionResult<AdminDTO>> CreateAdmin([FromBody] AdminCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password); // hashes the password

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var admin = new Admin
            {
                DisplayName = model.DisplayName,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = user.PasswordHash, // link the hashed password from the IdentityUser
                RegistrationDate = DateTime.UtcNow
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            var dto = new AdminDTO
            {
                AdminId = admin.AdminId,
                DisplayName = admin.DisplayName,
                Username = admin.Username,
                Email = admin.Email,
                RegistrationDate = admin.RegistrationDate
            };

            return CreatedAtAction(nameof(GetAdmin), new { id = admin.AdminId }, dto);
        }

        // GET by Id (placeholder)
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDTO>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return new AdminDTO
            {
                AdminId = admin.AdminId,
                DisplayName = admin.DisplayName,
                Username = admin.Username,
                Email = admin.Email,
                RegistrationDate = admin.RegistrationDate
            };
        }

        // other POST, PUT, DELETE methods if needed
    }
}
