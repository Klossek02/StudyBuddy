using static StudyBuddy.Controllers.StudentController;
using StudyBuddy.Models;
using StudyBuddy.Resources;
using StudyBuddy.DTO;
using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentResource _studentResource;
        private readonly PasswordHasher<StudentCreateModel> _passwordHasher;
        public StudentService(IStudentResource studentResource, PasswordHasher<StudentCreateModel> passwordHasher)
        {
            _studentResource = studentResource;
            _passwordHasher = passwordHasher;
        }

        public async Task<Student> CreateStudentAsync(StudentCreateModel model, string userId)
        {
            string hashedPassword = _passwordHasher.HashPassword(model, model.Password);

            var student = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = hashedPassword,
                EmailVerified = false, // default value
                RegistrationDate = DateTime.UtcNow,
                UserId = userId
            };

            return await _studentResource.CreateStudent(student);                        
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentCreateModel model)
        {
            return await _studentResource.UpdateStudent(id, model);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            return await _studentResource.GetStudent(id);
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            return await _studentResource.GetStudents();
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentResource.DeleteStudent(id);
        }
    }

}
