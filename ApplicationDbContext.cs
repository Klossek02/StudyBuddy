using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Models;

namespace StudyBuddy
{
    // class for handling a database instances
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } // DbSet for Students
        public DbSet<Tutor> Tutors { get; set; }  // DbSet for Tutors
        public DbSet<Admin> Admins { get; set; }  // DbSet for Admins
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Lesson> Lessons { get; set; } // DbSet for Lessons
    }
}
