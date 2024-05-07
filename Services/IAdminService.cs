using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public interface IAdminService
    {
        Task<AdminDto?> GetAdminByIdAsync(int id);
        Task<IEnumerable<AdminDto>> GetAllAdminsAsync();
    }
}
