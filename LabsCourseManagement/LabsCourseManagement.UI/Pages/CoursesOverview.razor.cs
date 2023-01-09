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
        public Guid removeProgramId { get; set; }
        public Guid removeAnnouncementId { get; set; }
        public Guid removeGradingId { get; set; }
        public Guid removeMaterialId { get; set; }

        public string? announcementHeader { get; set; }
        public string? announcementText { get; set; }
        public string? updateMaterialString { get; set; }
        public bool viewCourseSectionIsActive { get; set; }

        public string? programClassroom { get; set; }
        public string? programDateTime { get; set; }
        public string? gradingExaminationType { get; set; }
        public string? gradingIsMandatory { get; set; }
        public double gradingMinGrade { get; set; }
        public double gradingMaxGrade { get; set; }
        public string? gradingDescription { get; set; }
        public string? gradingClassroom { get; set; }
        public string? gradingDateTime { get; set; }


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
            await CourseDataService.AddMaterialsToCourse(updateCourseId, new List<string> { updateMaterialString });
            await OnInitializedAsync();
        }

        private async Task RemoveMaterialFromCourse()
        {
            await CourseDataService.RemoveMaterialsFromCourse(updateCourseId, new List<Guid> { removeMaterialId });
            await OnInitializedAsync();
        }

        private async Task AddAnnouncementToCourse()
        {
            var announcementInput = new AnnouncementInput{
                Header = announcementHeader,
                ProfessorId = announcementProfessorId,
                Text = announcementText
            };
            await CourseDataService.AddAnnouncementsToCourse(updateCourseId, new List<AnnouncementInput> { announcementInput });
            await OnInitializedAsync();
        }

        private async Task AddProgramToCourse()
        {
            var programInput = new TimeAndPlaceInput
            {
                Classroom = programClassroom,
                DateTime = programDateTime
            };
            await CourseDataService.AddProgramsToCourse(updateCourseId, new List<TimeAndPlaceInput> { programInput });
            await OnInitializedAsync();
        }

        private async Task RemoveProgramFromCourse()
        {
            await CourseDataService.RemoveProgramsFromCourse(updateCourseId, new List<Guid> { removeProgramId });
            await OnInitializedAsync();
        }

        private async Task RemoveAnnouncementFromCourse()
        {
            await CourseDataService.RemoveAnnouncementsFromCourse(updateCourseId, new List<Guid> { removeAnnouncementId });
            await OnInitializedAsync();
        }

        private async Task AddGradingToCourse()
        {
            var mandatory = true;
            if (gradingIsMandatory != "y")
            {
                mandatory = false;
            }

            var gradingInput = new GradingInput
            {
                Classroom = gradingClassroom,
                DateTime = gradingDateTime,
                MinGrade = gradingMinGrade,
                MaxGrade = gradingMaxGrade,
                Description = gradingDescription,
                ExaminationType = gradingExaminationType,
                IsMandatory = mandatory
            };
            await CourseDataService.AddGradingsToCourse(updateCourseId, new List<GradingInput> { gradingInput });
            await OnInitializedAsync();
        }

        private async Task RemoveGradingFromCourse()
        {
            await CourseDataService.RemoveGradingsFromCourse(updateCourseId, new List<Guid> { removeGradingId });
            await OnInitializedAsync();
        }
    }
}
