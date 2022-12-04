using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IProfDataService
    {
        Task<IEnumerable<ProfessorModel>> GetAllProfessors();
        Task<ProfessorModel> GetProfessorDetail(Guid professorId);
    }
}
