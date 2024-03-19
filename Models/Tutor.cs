namespace StudyBuddy.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailVerified { get; set; } = false;
        public string ExpertiseArea { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
