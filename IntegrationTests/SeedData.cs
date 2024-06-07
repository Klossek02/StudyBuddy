using Microsoft.AspNetCore.Identity;
using StudyBuddy.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
using StudyBuddy;

namespace StudyBuddy.IntegrationTests
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seeding students
            var student1 = new Student
            {
                StudentId = 1,
                FirstName = "James",
                LastName = "Hetfield",
                Email = "james.@hetfield.com"
            };
            var student2 = new Student
            {
                StudentId = 2,
                FirstName = "Jim",
                LastName = "Morrison",
                Email = "jim.morrison@gmail.com"
            };

            context.Students.AddRange(student1, student2);

            // Creating users for students
            var studentUser1 = new IdentityUser
            {
                UserName = student1.Email,
                Email = student1.Email
            };
            userManager.CreateAsync(studentUser1, "FightFIREwithFIRE#").Wait();
            student1.PasswordHash = userManager.PasswordHasher.HashPassword(studentUser1, "FightFIREwithFIRE#");

            var studentUser2 = new IdentityUser
            {
                UserName = student2.Email,
                Email = student2.Email
            };
            userManager.CreateAsync(studentUser2, "CRYst@l67SHIP").Wait();
            student2.PasswordHash = userManager.PasswordHasher.HashPassword(studentUser2, "CRYst@l67SHIP");

            // Seeding tutors
            var tutor1 = new Tutor
            {
                TutorId = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                ExpertiseArea = "Mathematics"
            };
            var tutor2 = new Tutor
            {
                TutorId = 2,
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                ExpertiseArea = "Physics"
            };

            context.Tutors.AddRange(tutor1, tutor2);

            // Creating users for tutors
            var tutorUser1 = new IdentityUser
            {
                UserName = tutor1.Email,
                Email = tutor1.Email
            };
            userManager.CreateAsync(tutorUser1, "AliceSecureP@ssword1").Wait();
            tutor1.PasswordHash = userManager.PasswordHasher.HashPassword(tutorUser1, "AliceSecureP@ssword1");

            var tutorUser2 = new IdentityUser
            {
                UserName = tutor2.Email,
                Email = tutor2.Email
            };
            userManager.CreateAsync(tutorUser2, "BobSecureP@ssword2").Wait();
            tutor2.PasswordHash = userManager.PasswordHasher.HashPassword(tutorUser2, "BobSecureP@ssword2");

            // Seeding admins
            var admin1 = new Admin
            {
                AdminId = 1,
                DisplayName = "Admin One",
                Email = "admin.one@example.com",
                Username = "adminone"
            };
            var admin2 = new Admin
            {
                AdminId = 2,
                DisplayName = "Admin Two",
                Email = "admin.two@example.com",
                Username = "admintwo"
            };

            context.Admins.AddRange(admin1, admin2);

            // Creating users for admins
            var adminUser1 = new IdentityUser
            {
                UserName = admin1.Email,
                Email = admin1.Email
            };
            userManager.CreateAsync(adminUser1, "AdminSecureP@ssword1").Wait();
            admin1.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser1, "AdminSecureP@ssword1");

            var adminUser2 = new IdentityUser
            {
                UserName = admin2.Email,
                Email = admin2.Email
            };
            userManager.CreateAsync(adminUser2, "AdminSecureP@ssword2").Wait();
            admin2.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser2, "AdminSecureP@ssword2");

            // Seeding lessons
            context.Lessons.AddRange(
                new Lesson
                {
                    LessonId = 1,
                    StudentId = student1.StudentId,
                    TutorId = tutor1.TutorId,
                    Subject = "Math",
                    LessonDate = DateTime.UtcNow.AddDays(1),
                    State = LessonState.Requested
                },
                new Lesson
                {
                    LessonId = 2,
                    StudentId = student2.StudentId,
                    TutorId = tutor2.TutorId,
                    Subject = "Science",
                    LessonDate = DateTime.UtcNow.AddDays(2),
                    State = LessonState.Requested
                }
            );

            context.SaveChanges();
        }
    }
}
