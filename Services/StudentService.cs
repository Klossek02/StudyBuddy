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
        private readonly UserManager<IdentityUser> _userManager;

        public StudentService(IStudentResource studentResource, UserManager<IdentityUser> userManager)
        {
            _studentResource = studentResource;
            _userManager = userManager;
        }

        public async Task<Student> CreateStudentAsync(StudentCreateModel model)
        {
            // Business logic/validation can be added here if needed

            var user = new IdentityUser { UserName = model.FirstName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password); // hashes the password

            if (result.Succeeded)
            {
                var student = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = user.PasswordHash,
                    EmailVerified = false, // default value
                    RegistrationDate = DateTime.UtcNow
                };
                return await _studentResource.CreateStudent(student);
            } 
            else
            {
                await Console.Out.WriteLineAsync("error hashing the password");
            }
            return new Student();
            
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentCreateModel model)
        {
            // Business logic/validation can be added here if needed

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

        public Task<bool> DeleteStudentAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
