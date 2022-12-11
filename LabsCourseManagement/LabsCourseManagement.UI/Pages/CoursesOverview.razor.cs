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
       
        public CourseInput NewCourse { get; set; } = new CourseInput();
        public List<CourseModel> Courses { get; set; } = default!;
        public List<ProfessorModel> Professors { get; set; } = default!;

        public Guid id { get; set; }
        public Guid updateCourseId { get; set; }
        public Guid updateProfessorId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
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
            await CourseDataService.AddProfessorToCourse(updateCourseId, updateProfessorId);
        }
    }
}
