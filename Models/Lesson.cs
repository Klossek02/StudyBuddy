namespace StudyBuddy.Models
{
    public class Lesson
    {
        public Lesson()
        {
            State = LessonState.Requested; // Set default value to Requested
            Link = null; // Set Link property to null by default
        }
        public int LessonId { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public string Subject { get; set; }
        public DateTime LessonDate { get; set; }
        public LessonState State { get; set; } // Enum for lesson state
        public string? Link { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Tutor Tutor { get; set; }
    }

    public enum LessonState
    {
        Requested,
        Accepted,
        Rejected
    }
}
