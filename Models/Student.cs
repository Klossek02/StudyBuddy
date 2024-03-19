namespace StudyBuddy.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailVerified { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

    }
}
