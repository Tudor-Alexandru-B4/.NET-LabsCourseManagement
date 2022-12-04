using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IProfDataService
    {
        Task CreateProfessor(ProfessorCreateModel professor);
        Task DeleteProfessor(Guid professorId);
        Task<IEnumerable<ProfessorModel>> GetAllProfessors();
        Task<ProfessorModel> GetProfessorDetail(Guid professorId);
    }
}
