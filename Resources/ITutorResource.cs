﻿using StudyBuddy.DTO;
using StudyBuddy.Models;

namespace StudyBuddy.Resources
{
    public interface ITutorResource
    {
        Task<Tutor> CreateTutor(Tutor tutor);
        Task<bool> UpdateTutor(int id, TutorCreateModel model);
        Task<TutorDto?> GetTutor(int id);
        Task<IEnumerable<TutorDto>> GetTutors();

        Task<bool> DeleteTutor(int id);
    }
}
