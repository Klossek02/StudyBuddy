using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _context.Students
                .Select(student => new
                {
                    student.StudentId,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.EmailVerified,
                    student.RegistrationDate
                })
                .ToListAsync();

            return Ok(students);
        }

        public class StudentCreateModel
        {
            [Required]
            [StringLength(100)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                EmailVerified = false, // default value
                RegistrationDate = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            // not updating EmailVerified and RegistrationDate here

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students
                .Select(student => new
                {
                    student.StudentId,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.EmailVerified,
                    student.RegistrationDate
                })
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // other POST, PUT, DELETE methods if needed
    }


    [Route("api/[controller]")]
    [ApiController]
    public class TutorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TutorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class TutorCreateModel
        {
            [Required]
            [StringLength(100)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string ExpertiseArea { get; set; }
        }

        public class TutorDTO
        {
            public int TutorId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public bool EmailVerified { get; set; }
            public string ExpertiseArea { get; set; }
            public DateTime RegistrationDate { get; set; }
        }

        // GET: api/Tutors
        [HttpGet]
        public async Task<IActionResult> GetTutors()
        {
            var tutors = await _context.Tutors
                .Select(tutor => new TutorDTO
                {
                    TutorId = tutor.TutorId,
                    FirstName = tutor.FirstName,
                    LastName = tutor.LastName,
                    Email = tutor.Email,
                    EmailVerified = tutor.EmailVerified,
                    ExpertiseArea = tutor.ExpertiseArea,
                    RegistrationDate = tutor.RegistrationDate
                })
                .ToListAsync();

            return Ok(tutors);
        }

        // GET: api/Tutors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTutor(int id)
        {
            var tutor = await _context.Tutors
                .Where(t => t.TutorId == id)
                .Select(tutor => new TutorDTO
                {
                    TutorId = tutor.TutorId,
                    FirstName = tutor.FirstName,
                    LastName = tutor.LastName,
                    Email = tutor.Email,
                    EmailVerified = tutor.EmailVerified,
                    ExpertiseArea = tutor.ExpertiseArea,
                    RegistrationDate = tutor.RegistrationDate
                })
                .FirstOrDefaultAsync();

            if (tutor == null)
            {
                return NotFound();
            }

            return Ok(tutor);
        }

        // POST: api/Tutors
        [HttpPost]
        public async Task<IActionResult> CreateTutor([FromBody] TutorCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tutor = new Tutor
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ExpertiseArea = model.ExpertiseArea,
                EmailVerified = false, // default value
                RegistrationDate = DateTime.UtcNow
            };

            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutor);
        }

        // PUT: api/Tutors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] TutorCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            tutor.FirstName = model.FirstName;
            tutor.LastName = model.LastName;
            tutor.Email = model.Email;
            tutor.ExpertiseArea = model.ExpertiseArea;
            // not updating EmailVerified and RegistrationDate here as well

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Tutors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            _context.Tutors.Remove(tutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // other POST, PUT, DELETE methods if needed
    }



    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // for handling passwords securely

        public AdminsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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