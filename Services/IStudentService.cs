using static StudyBuddy.Controllers.StudentController;
using StudyBuddy.Models;
using StudyBuddy.DTO;

namespace StudyBuddy.Services
{
    public interface IStudentService
    {
        // Define methods for student operations
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<Student> CreateStudentAsync(StudentCreateModel model, string userId);
        Task<bool> UpdateStudentAsync(int id, StudentCreateModel model);
        Task<bool> DeleteStudentAsync(int id);
    }

}
