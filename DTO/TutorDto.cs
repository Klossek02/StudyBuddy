namespace StudyBuddy.DTO
{
    public class TutorDto
    {
        public int TutorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string ExpertiseArea { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
