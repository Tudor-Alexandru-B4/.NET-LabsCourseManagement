using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class CoursesOverview : ComponentBase
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; } = default!;

        [Inject]
        public IProfDataService ProfDataService { get; set; } = default!;
        [Inject]
        public IStudentDataService StudentDataService { get; set; } = default!;

        public CourseInput NewCourse { get; set; } = new CourseInput();
        public List<CourseModel> Courses { get; set; } = new List<CourseModel>();
        public List<ProfessorModel> Professors { get; set; } = new List<ProfessorModel>();
        public List<StudentModel> Students { get; set; } = new List<StudentModel>();

        public string MaterialLink { get; set; } = default!;
        public string AnnouncementHeader { get; set; } = default!;
        public string AnnouncementText { get; set; } = default!;

        public Guid id { get; set; }
        public Guid updateCourseId { get; set; }
        public Guid updateProfessorId { get; set; }
        public Guid updateStudentId { get; set; }
        public Guid updateMaterialId { get; set; }
        public Guid updateAnnouncementId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Students = (await StudentDataService.GetAllStudents() ?? new List<StudentModel>()).ToList();
        }
        private async Task CreateCourse()
        {
            await CourseDataService.CreateCourse(NewCourse);
        }
        private async Task DeleteCourse()
        {
            await CourseDataService.DeleteCourse(id);
        }

        private async Task AddProfessorToCourse()
        {
            await CourseDataService.AddProfessorsToCourse(updateCourseId, new List<Guid> { updateProfessorId });
        }

        private async Task AddStudentsToCourse()
        {
            await CourseDataService.AddStudentsToCourse(updateCourseId, new List<Guid> { updateStudentId });
        }
    }
}
