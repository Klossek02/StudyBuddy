//using static StudyBuddy.Controllers.StudentController;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Resources
{
    public class StudentResource : IStudentResource
    {
        private readonly ApplicationDbContext _context;

        public StudentResource(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudent(int id, StudentCreateModel model)
        {
            // Implement update logic using EF Core
            var student = await _context.Students.FindAsync(id);

            if (student != null) 
            { 
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Email = model.Email;
                // not updating EmailVerified and RegistrationDate here
            }
            else
            {
                return false;
            }            

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentDto?> GetStudent(int id)
        {
            // Implement retrieval logic using EF Core
            return await _context.Students
                .Select(student => new StudentDto
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    EmailVerified = student.EmailVerified,
                    RegistrationDate = student.RegistrationDate
                })
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<IEnumerable<StudentDto>> GetStudents()
        {
            return await _context.Students
                .Select(student => new StudentDto
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    EmailVerified = student.EmailVerified,
                    RegistrationDate = student.RegistrationDate
                })
                .ToListAsync();
        }
    }
}
