using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManage.UI.Pages.Services
{
    public interface ILaboratoryDataService
    {
        Task<IEnumerable<Laboratory>> GetAllLaboratories();
        Task<Laboratory> GetLaboratoryDetails(Guid laboratoryId);
    }
}