using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface IStudentDataService
    {
        Task CreateStudent(StudentCreateModel student);
        Task DeleteStudent(Guid studentId);
        Task<IEnumerable<StudentModel>?> GetAllStudents();
        Task UpdateName(Guid studentId, string name);
        Task UpdateSurname(Guid studentId, string name);
        Task UpdateGroup(Guid studentId, string groupName);
        Task UpdateYear(Guid studentId, int year);
        Task UpdateRegistrationNumber(Guid studentId, string registrationNumber);
    }
}
