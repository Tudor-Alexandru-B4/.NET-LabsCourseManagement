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
        public CourseModel? CourseToView { get; set; }

        public string MaterialLink { get; set; } = default!;
        public string UpdateCourseName { get; set; } = default!;

        public Guid updateCourseId { get; set; }
        public Guid updateProfessorId { get; set; }
        public Guid updateStudentId { get; set; }
        public Guid updateMaterialId { get; set; }
        public Guid announcementProfessorId { get; set; }
        public Guid removeProfessorId { get; set; }
        public Guid removeStudentId { get; set; }
        public string? announcementHeader { get; set; }
        public string? announcementText { get; set; }
        public bool viewCourseSectionIsActive { get; set; }


        private async Task RemoveStudentsFromCourse()
        {
            await CourseDataService.RemoveStudentsFromCourse(updateCourseId, new List<Guid> { removeStudentId });
            await OnInitializedAsync();
        }

        private async Task RemoveProfessorFromCourse()
        {
            await CourseDataService.RemoveProfessorsFromCourse(updateCourseId, new List<Guid> { removeProfessorId });
            await OnInitializedAsync();
        }

        private async Task UpdateName()
        {
            await CourseDataService.UpdateName(updateCourseId, UpdateCourseName);
            await OnInitializedAsync();
        }

        private async Task UpdateActiveStatus(bool newStatus)
        {
            await CourseDataService.UpdateActiveStatus(updateCourseId, newStatus);
            await OnInitializedAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Students = (await StudentDataService.GetAllStudents() ?? new List<StudentModel>()).ToList();
            CourseToView = Courses.FirstOrDefault(c => c.Id == updateCourseId);
        }

        private async Task CreateCourse()
        {
            await CourseDataService.CreateCourse(NewCourse);
            await OnInitializedAsync();
        }

        private async Task DeleteCourse(Guid courseId)
        {
            updateCourseId = courseId;
            await CourseDataService.DeleteCourse(updateCourseId);
            await OnInitializedAsync();
        }

        private void ViewCourse(Guid courseId)
        {
            updateCourseId = courseId;
            viewCourseSectionIsActive = true;
            CourseToView = Courses.FirstOrDefault(c => c.Id == updateCourseId);
        }

        private void CloseView()
        {
            viewCourseSectionIsActive = false;
        }

        private async Task AddProfessorToCourse()
        {
            await CourseDataService.AddProfessorsToCourse(updateCourseId, new List<Guid> { updateProfessorId });
            await OnInitializedAsync();
        }

        private async Task AddStudentsToCourse()
        {
            await CourseDataService.AddStudentsToCourse(updateCourseId, new List<Guid> { updateStudentId });
            await OnInitializedAsync();
        }

        private async Task AddMaterialsToCourse()
        {
            await CourseDataService.AddMaterialsToCourse(updateCourseId, new List<Guid> { updateMaterialId });
        }

        private async Task AddAnnouncementToCourse()
        {
            var announcementInput = new AnnouncementInput{
                Header = announcementHeader,
                ProfessorId = announcementProfessorId,
                Text = announcementText
            };
            await CourseDataService.AddAnnouncementsToCourse(updateCourseId, new List<AnnouncementInput> { announcementInput });
        }
    }
}
