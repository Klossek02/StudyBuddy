using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using static StudyBuddy.Controllers.AdminController;

namespace StudyBuddy.Resources
{
    public class AdminResource : IAdminResource
    {
        private readonly ApplicationDbContext _context;

        public AdminResource(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDto?> GetAdmin(int id)
        {
            return await _context.Admins
                .Select(admin => new AdminDto
                {
                    AdminId = admin.AdminId,
                    DisplayName = admin.DisplayName,
                    Username = admin.Username,
                    Email = admin.Email,
                    RegistrationDate = admin.RegistrationDate
                })
                .FirstOrDefaultAsync(s => s.AdminId == id);
        }

        public async Task<IEnumerable<AdminDto>> GetAdmins()
        {
            return await _context.Admins
                .Select(a => new AdminDto
                {
                    AdminId = a.AdminId,
                    DisplayName = a.DisplayName,
                    Username = a.Username,
                    Email = a.Email,
                    RegistrationDate = a.RegistrationDate
                })
                .ToListAsync();
        }
    }
}
