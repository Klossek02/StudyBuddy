namespace StudyBuddy.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
