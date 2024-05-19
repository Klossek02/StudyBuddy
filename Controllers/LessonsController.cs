using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestLesson([FromBody] CreateLessonDto lessonDto)
        {
            // Logic to request a lesson (for students)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lesson = await _lessonService.CreateLessonAsync(lessonDto);
            return CreatedAtAction(nameof(GetLessonById), new { id = lesson.LessonId }, lesson);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }


        [HttpPut("{lessonId}/accept")]
        public async Task<IActionResult> AcceptLesson(int lessonId)
        {
            var lesson = await _lessonService.AcceptLessonAsync(lessonId);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        [HttpPut("{lessonId}/reject")]
        public async Task<IActionResult> RejectLesson(int lessonId)
        {
            // Logic to reject a lesson request (for tutors)
            var lesson = await _lessonService.RejectLessonAsync(lessonId);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        // GET: api/lessons
        [HttpGet]
        public async Task<IActionResult> GetLessons(string type)
        {
            // Get the user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get the user's roles from the claims
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (userId == null)
            {
                return Unauthorized();
            }

            IEnumerable<Lesson> lessons;

            if (roles.Contains("Student"))
            {
                // Assuming type is a valid LessonState string
                if (!Enum.TryParse(type, out LessonState lessonState))
                {
                    return BadRequest("Invalid lesson type.");
                }

                lessons = await _lessonService.GetLessonsForStudentAsync(int.Parse(userId), lessonState);
            }
            else if (roles.Contains("Tutor"))
            {
                // Assuming type is a valid LessonState string
                if (!Enum.TryParse(type, out LessonState lessonState))
                {
                    return BadRequest("Invalid lesson type.");
                }

                lessons = await _lessonService.GetLessonsForTutorAsync(int.Parse(userId), lessonState);
            }
            else
            {
                return Unauthorized();
            }

            return Ok(lessons);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonDto lessonDto)
        {
            var lesson = await _lessonService.UpdateLessonAsync(id, lessonDto);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

    }
}
