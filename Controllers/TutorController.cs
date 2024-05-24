using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService _tutorService;
        private readonly UserManager<IdentityUser> _userManager;

        public TutorController(ITutorService tutorService, UserManager<IdentityUser> userManager)
        {
            _tutorService = tutorService;
            _userManager = userManager; 
        }

        // GET: api/Tutors
        [HttpGet]
        public async Task<IActionResult> GetTutors()
        {
            IEnumerable<TutorDto> tutors = await _tutorService.GetAllTutorsAsync();

            return Ok(tutors);
        }

        // GET: api/Tutors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTutor(int id)
        {
            var tutor = await _tutorService.GetTutorByIdAsync(id);

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

            // Create the user
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the "tutor" role
            var roleResult = await _userManager.AddToRoleAsync(user, "Tutor");
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            var tutor = await _tutorService.CreateTutorAsync(model, user.Id);

            return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutor);
        }

        // PUT: api/Tutors/{id} -- Update tutor details including settings (user setting adjustment)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] TutorCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tutor = await _tutorService.GetTutorByIdAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            bool success = await _tutorService.UpdateTutorAsync(id, model);
            if (!success)
            {
                return StatusCode(500, "Unable to update the tutor information."); // more specific error explanation
            }

            return NoContent(); // successfully updated
        }

        // DELETE: api/Tutors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            var success = await _tutorService.DeleteTutorAsync(id);
            if (!success)
            {
                return NotFound(); // indicate that the tutor was not found
            }

            return NoContent();
        }
        // other POST, PUT, DELETE methods if needed
    }
}
