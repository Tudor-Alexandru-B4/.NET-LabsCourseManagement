using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class CoursesOverview : ComponentBase
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public IProfDataService ProfDataService { get; set; }
       
        public CourseInput NewCourse = new CourseInput();
        public List<CourseModel> Courses { get; set; } = default!;

        public List<ProfessorModel> Professors { get; set; } = default!;

        public Guid id;
        public Guid updateCourseId;
        public Guid updateProfessorId;
        
        protected override async Task OnInitializedAsync()
        {
            Courses = (await CourseDataService.GetAllCourses()).ToList();
            Professors = (await ProfDataService.GetAllProfessors()).ToList();
        }
        private async void CreateCourse()
        {
            await CourseDataService.CreateCourse(NewCourse);
        }
        private async void DeleteCourse()
        {
            await CourseDataService.DeleteCourse(id);
        }

        private async void AddProfessorToCourse()
        {
            await CourseDataService.AddProfessorToCourse(updateCourseId, updateProfessorId);
        }
    }
}
