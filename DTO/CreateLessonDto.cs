using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTO
{
    public class CreateLessonDto
    {
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public string Subject { get; set; }
        public DateTime LessonDate { get; set; }
    }
}
