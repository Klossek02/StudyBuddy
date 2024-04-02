using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Services;
using StudyBuddy.Models;
using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSettingsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserSettingsController(IStudentService studentService, ITutorService tutorService, UserManager<IdentityUser> userManager)
        {
            _studentService = studentService;
            _tutorService = tutorService;
            _userManager = userManager;
        }

        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _studentService.UpdateStudentAsync(id, model);

            if (!success)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }

        [HttpPut("update-tutor/{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] Tutor model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _tutorService.UpdateTutorAsync(id, model);

            if (!success)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }

    }
}
