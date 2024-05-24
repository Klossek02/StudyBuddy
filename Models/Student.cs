namespace StudyBuddy.Models
{
// defining Student fields
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailVerified { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Lesson> Lessons { get; set; }

        public string UserId { get; set; }

    }
}
