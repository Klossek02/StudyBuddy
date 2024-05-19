using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons.FindAsync(id);
        }
        public async Task<Lesson> CreateLessonAsync(CreateLessonDto model)
        {
            var lesson = new Lesson
            {
                StudentId = model.StudentId,
                TutorId = model.TutorId,
                Subject = model.Subject,
                LessonDate = model.LessonDate,
                State = LessonState.Requested, // Default state
                Link = null // Initially null
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return lesson;
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return false;
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Lesson>> GetLessonsForStudentAsync(int studentId, LessonState lessonState)
        {
            return await _context.Lessons
                .Where(l => l.StudentId == studentId && l.State == lessonState)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lesson>> GetLessonsForTutorAsync(int tutorId, LessonState lessonState)
        {
            return await _context.Lessons
                .Where(l => l.TutorId == tutorId && l.State == lessonState)
                .ToListAsync();
        }

        public async Task<Lesson?> UpdateLessonAsync(int id, UpdateLessonDto model)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return null;
            }
            
            lesson.State = model.State;
            lesson.LessonDate = model.LessonDate;
            await _context.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson?> AcceptLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (lesson == null)
            {
                return null;
            }

            lesson.State = LessonState.Accepted;
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson?> RejectLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (lesson == null)
            {
                return null;
            }

            lesson.State = LessonState.Rejected;
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();

            return lesson;
        }
    }
}
