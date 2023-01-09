using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface ILaboratoryDataService
    {
        Task<IEnumerable<LaboratoryModel>?> GetAllLaboratories();
        Task CreateLaboratory(LaboratoryInput laboratory);
        Task DeleteLaboratory(Guid laboratoryId);
        Task AddStudentsToLaboratory(Guid laboratoryId, List<Guid> studentsIds);
        Task UpdateName(Guid laboratoryId, string name);
        Task UpdateActiveStatus(Guid laboratoryId, bool activeStatus);
        Task RemoveStudentsFromLaboratory(Guid laboratoryId, List<Guid> studentsIds);
        Task AddAnnouncementsToLaboratory(Guid laboratoryId, List<AnnouncementInput> announcementsInputs);
        Task RemoveAnnouncementsFromLaboratory(Guid laboratoryId, List<Guid> announcementsIds);
        Task AddGradingsToLaboratory(Guid laboratoryId, List<GradingInput> gradingsInputs);
        Task RemoveGradingsFromLaboratory(Guid laboratoryId, List<Guid> gradingsIds);
        Task UpdateProfessorFromLaboratory(Guid laboratoryId, Guid professorId);
    }
}