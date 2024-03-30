using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Models;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TutorController(ApplicationDbContext context)
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
                PasswordHash = "#####",
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
}
