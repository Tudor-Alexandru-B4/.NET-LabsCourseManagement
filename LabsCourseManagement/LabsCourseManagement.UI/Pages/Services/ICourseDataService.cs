using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;

namespace LabsCourseManagement.UI.Pages.Services
{
    public interface ICourseDataService
    {
        Task CreateCourse(CourseInput course);
        Task DeleteCourse(Guid courseId);
        Task<IEnumerable<CourseModel>?> GetAllCourses();
        Task AddProfessorsToCourse(Guid courseId, List<Guid> professorsIds);
        Task AddStudentsToCourse(Guid courseId, List<Guid> studentsIds);
        Task UpdateName(Guid courseId, string name);
        Task UpdateActiveStatus(Guid courseId, bool activeStatus);
        Task RemoveProfessorsFromCourse(Guid courseId, List<Guid> professorsIds);
        Task RemoveStudentsFromCourse(Guid courseId, List<Guid> studentsIds);
        Task AddMaterialsToCourse(Guid courseId, List<string> materials);
        Task RemoveMaterialsFromCourse(Guid courseId, List<Guid> materialsIds);
        Task AddAnnouncementsToCourse(Guid courseId, List<AnnouncementInput> announcementsInputs);
        Task RemoveAnnouncementsFromCourse(Guid courseId, List<Guid> announcementsIds);
        Task AddProgramsToCourse(Guid courseId, List<TimeAndPlaceInput> timeAndPlaceInputs);
        Task RemoveProgramsFromCourse(Guid courseId, List<Guid> programsIds);
        Task AddGradingsToCourse(Guid courseId, List<GradingInput> gradingsInputs);
        Task RemoveGradingsFromCourse(Guid courseId, List<Guid> gradingsIds);
    }
}