using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface ILaboratoryDataService
    {
        Task<IEnumerable<LaboratoryModel>?> GetAllLaboratories();
    }
}