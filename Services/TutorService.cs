using Microsoft.AspNetCore.Identity;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Resources;

namespace StudyBuddy.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorResource _tutorResource;
        private readonly PasswordHasher<TutorCreateModel> _passwordHasher;

        public TutorService(ITutorResource tutorResource, PasswordHasher<TutorCreateModel> passwordHasher)
        {
            _tutorResource = tutorResource;
            _passwordHasher = passwordHasher;
        }
        public async Task<Tutor> CreateTutorAsync(TutorCreateModel model)
        {
            string hashedPassword = _passwordHasher.HashPassword(model, model.Password);

            var tutor = new Tutor
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = hashedPassword,
                ExpertiseArea = model.ExpertiseArea,
                EmailVerified = false, // default value
                RegistrationDate = DateTime.UtcNow
            };

            return await _tutorResource.CreateTutor(tutor);
        }

        public async Task<bool> DeleteTutorAsync(int id)
        {
            return await _tutorResource.DeleteTutor(id);
        }

        public async Task<IEnumerable<TutorDto>> GetAllTutorsAsync()
        {
            return await _tutorResource.GetTutors();
        }

        public async Task<TutorDto?> GetTutorByIdAsync(int id)
        {
            return await _tutorResource.GetTutor(id);
        }

        public async Task<bool> UpdateTutorAsync(int id, TutorCreateModel model)
        {
            return await _tutorResource.UpdateTutor(id, model);
        }
    }
}
