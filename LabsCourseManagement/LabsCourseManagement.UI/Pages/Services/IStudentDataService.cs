using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IStudentDataService
    {
        Task<IEnumerable<StudentModel>> GetAllStudent();
        Task<StudentModel> GetStudentDetail(Guid studentId);
    }
}
