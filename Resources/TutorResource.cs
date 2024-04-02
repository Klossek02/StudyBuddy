using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using static StudyBuddy.Controllers.TutorController;

namespace StudyBuddy.Resources
{
    public class TutorResource : ITutorResource
    {
        private readonly ApplicationDbContext _context;

        public TutorResource(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tutor> CreateTutor(Tutor tutor)
        {
            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();
            return tutor;
        }

        public async Task<bool> DeleteTutor(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return false;
            }

            _context.Tutors.Remove(tutor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TutorDto?> GetTutor(int id)
        {
            return await _context.Tutors
                .Where(t => t.TutorId == id)
                .Select(tutor => new TutorDto
                {
                    TutorId = tutor.TutorId,
                    FirstName = tutor.FirstName,
                    LastName = tutor.LastName,
                    Email = tutor.Email,
                    EmailVerified = tutor.EmailVerified,
                    ExpertiseArea = tutor.ExpertiseArea,
                    RegistrationDate = tutor.RegistrationDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TutorDto>> GetTutors()
        {
            return await _context.Tutors
                .Select(tutor => new TutorDto
                {
                    TutorId = tutor.TutorId,
                    FirstName = tutor.FirstName,
                    LastName = tutor.LastName,
                    Email = tutor.Email,
                    EmailVerified = tutor.EmailVerified,
                    ExpertiseArea = tutor.ExpertiseArea,
                    RegistrationDate = tutor.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateTutor(int id, TutorCreateModel model)
        {
            var tutor = await _context.Tutors.FindAsync(id);

            if (tutor != null)
            {
                tutor.FirstName = model.FirstName;
                tutor.LastName = model.LastName;
                tutor.Email = model.Email;
                tutor.ExpertiseArea = model.ExpertiseArea;
                // not updating EmailVerified and RegistrationDate here as well
            }
            else
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
