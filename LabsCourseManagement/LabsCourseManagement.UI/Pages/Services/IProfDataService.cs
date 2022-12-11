using LabsCourseManagement.Domain;
using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IProfDataService
    {
        Task CreateProfessor(ProfessorCreateModel professor);
        Task DeleteProfessor(Guid professorId);
        Task UpdateProfessorPhoneNumber(Guid professorId, Guid contactId, string phoneNumber);
        Task<IEnumerable<ProfessorModel>> GetAllProfessors();
        Task<ProfessorModel> GetProfessorDetail(Guid professorId);
        Task AddCourse(Guid courseId, Guid professorId);
    }
}
