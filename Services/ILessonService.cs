using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public interface ILessonService
    {
        Task<Lesson?> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetLessonsForStudentAsync(int studentId, LessonState lessonState);
        Task<IEnumerable<Lesson>> GetLessonsForTutorAsync(int tutorId, LessonState lessonState);
        Task<Lesson> CreateLessonAsync(CreateLessonDto model);
        Task<Lesson?> UpdateLessonAsync(int id, UpdateLessonDto model); 
        Task<bool> DeleteLessonAsync(int id);
        Task<Lesson?> AcceptLessonAsync(int lessonId);
        Task<Lesson?> RejectLessonAsync(int lessonId);
    }
}
