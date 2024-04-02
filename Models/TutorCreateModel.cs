using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class TutorCreateModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string ExpertiseArea { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
