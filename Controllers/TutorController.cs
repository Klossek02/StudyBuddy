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

        public TutorController(ITutorService tutorService)
        {
            _tutorService = tutorService;
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

            var tutor = await _tutorService.CreateTutorAsync(model);

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
