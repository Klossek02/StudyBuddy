using StudyBuddy.Models;

namespace StudyBuddy.DTO
{
    public class UpdateLessonDto
    {
        public string Subject { get; set; }
        public DateTime LessonDate { get; set; }
        public LessonState State { get; set; }
        public string? Link { get; set; }
    }
}
