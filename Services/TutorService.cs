using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Resources;

namespace StudyBuddy.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorResource _tutorResource;

        public TutorService(ITutorResource tutorResource)
        {
            _tutorResource = tutorResource;
        }
        public async Task<Tutor> CreateTutorAsync(TutorCreateModel model)
        {
            return await _tutorResource.CreateTutor(model);
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
