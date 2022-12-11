using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IProfDataService
    {
        Task CreateProfessor(ProfessorCreateModel professor);
        Task DeleteProfessor(Guid professorId);
        Task UpdateProfessorPhoneNumber(Guid professorId, Guid contactId, string phoneNumber);
        Task<IEnumerable<ProfessorModel>?> GetAllProfessors();
        Task AddCourse(Guid courseId, Guid professorId);
    }
}
