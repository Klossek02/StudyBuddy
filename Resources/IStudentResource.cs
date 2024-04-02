using static StudyBuddy.Controllers.StudentController;
using StudyBuddy.Models;
using StudyBuddy.DTO;

namespace StudyBuddy.Resources
{
    public interface IStudentResource
    {
        Task<Student> CreateStudent(Student student);
        Task<bool> UpdateStudent(int id, StudentCreateModel model);
        Task<StudentDto?> GetStudent(int id);
        Task<IEnumerable<StudentDto>> GetStudents();
    }
}
