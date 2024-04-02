using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Resources
{
    public interface IAdminResource
    {
        Task<AdminDto?> GetAdmin(int id);
        Task<IEnumerable<AdminDto>> GetAdmins();
    }
}
