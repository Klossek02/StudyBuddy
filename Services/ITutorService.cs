using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public interface ITutorService
    {
        Task<TutorDto?> GetTutorByIdAsync(int id);
        Task<IEnumerable<TutorDto>> GetAllTutorsAsync();
        Task<Tutor> CreateTutorAsync(TutorCreateModel model, string userId);
        Task<bool> UpdateTutorAsync(int id, TutorCreateModel model);
        Task<bool> DeleteTutorAsync(int id);
    }
}
