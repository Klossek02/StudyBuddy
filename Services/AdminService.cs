using Microsoft.AspNetCore.Identity;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Resources;

namespace StudyBuddy.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminResource _adminResource;

        public AdminService(IAdminResource adminResource)
        {
            _adminResource = adminResource;
        }

        public async Task<AdminDto?> GetAdminByIdAsync(int id)
        {
            return await _adminResource.GetAdmin(id);
        }

        public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
        {
            return await _adminResource.GetAdmins();
        }

    }
}
