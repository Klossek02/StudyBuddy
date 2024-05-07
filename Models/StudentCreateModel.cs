using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class StudentCreateModel
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

        [Required]
        public string Password { get; set; } // for simplicity, assuming plain text to be hashed in service

    }
}
