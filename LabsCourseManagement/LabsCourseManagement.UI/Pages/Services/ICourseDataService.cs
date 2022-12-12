using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface ICourseDataService
    {
        Task CreateCourse(CourseInput course);
        Task DeleteCourse(Guid courseId);
        Task<IEnumerable<CourseModel>?> GetAllCourses();
        Task AddProfessorsToCourse(Guid courseId, List<Guid> professorId);
        Task AddStudentsToCourse(Guid courseId, List<Guid> studentsIds);

    }
}