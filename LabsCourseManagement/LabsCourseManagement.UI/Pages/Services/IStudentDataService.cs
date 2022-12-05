using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IStudentDataService
    {
        Task CreateStudent(StudentCreateModel student);
        Task DeleteStudent(Guid studentId);
        Task<IEnumerable<StudentModel>> GetAllStudent();
        Task<StudentModel> GetStudentDetail(Guid studentId);
        Task UpdateStudentGroup(Guid studentId, string groupName);
    }
}
