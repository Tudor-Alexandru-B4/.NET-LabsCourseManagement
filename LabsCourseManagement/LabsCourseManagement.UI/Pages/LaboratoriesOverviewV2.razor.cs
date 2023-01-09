using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class LaboratoriesOverviewV2 : ComponentBase
    {
        [Inject]
        public ILaboratoryDataService LaboratoryDataService { get; set; } = default!;
        [Inject]
        public ICourseDataService CourseDataService { get; set; } = default!;
        [Inject]
        public IProfDataService ProfDataService { get; set; } = default!;
        [Inject]
        public IStudentDataService StudentDataService { get; set; } = default!;

        public LaboratoryInput NewLaboratory { get; set; } = new LaboratoryInput();
        public List<CourseModel> Courses { get; set; } = new List<CourseModel>();
        public List<LaboratoryModel> Laboratories { get; set; } = new List<LaboratoryModel>();
        public List<ProfessorModel> Professors { get; set; } = new List<ProfessorModel>();
        public List<StudentModel> Students { get; set; } = new List<StudentModel>();
        public LaboratoryModel? LaboratoryToView { get; set; }

        public Guid updateLaboratoryId { get; set; }
        public Guid updateProfessorId { get; set; }
        public Guid updateStudentId { get; set; }
        public Guid announcementProfessorId { get; set; }
        public Guid removeStudentId { get; set; }
        public Guid removeAnnouncementId { get; set; }
        public Guid removeGradingId { get; set; }
        public Guid removeMaterialId { get; set; }

        public string? announcementHeader { get; set; }
        public string? announcementText { get; set; }
        public bool viewLaboratorySectionIsActive { get; set; }

        public string UpdateLaboratoryName { get; set; } = default!;

        public string? programClassroom { get; set; }
        public string? programDateTime { get; set; }

        public string? gradingExaminationType { get; set; }
        public string? gradingIsMandatory { get; set; }
        public double gradingMinGrade { get; set; }
        public double gradingMaxGrade { get; set; }
        public string? gradingDescription { get; set; }
        public string? gradingClassroom { get; set; }
        public string? gradingDateTime { get; set; }


        private async Task RemoveStudentsFromLaboratory()
        {
            await LaboratoryDataService.RemoveStudentsFromLaboratory(updateLaboratoryId, new List<Guid> { removeStudentId });
            await OnInitializedAsync();
        }

        private async Task UpdateName()
        {
            await LaboratoryDataService.UpdateName(updateLaboratoryId, UpdateLaboratoryName);
            await OnInitializedAsync();
        }

        private async Task UpdateActiveStatus(bool newStatus)
        {
            await LaboratoryDataService.UpdateActiveStatus(updateLaboratoryId, newStatus);
            await OnInitializedAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            Laboratories = (await LaboratoryDataService.GetAllLaboratories() ?? new List<LaboratoryModel>()).ToList();
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Students = (await StudentDataService.GetAllStudents() ?? new List<StudentModel>()).ToList();
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            LaboratoryToView = Laboratories.FirstOrDefault(l => l.Id == updateLaboratoryId);
        }

        private async Task CreateLaboratory()
        {
            await LaboratoryDataService.CreateLaboratory(NewLaboratory);
            await OnInitializedAsync();
        }

        private async Task DeleteLaboratory(Guid laboratoryId)
        {
            updateLaboratoryId = laboratoryId;
            await LaboratoryDataService.DeleteLaboratory(updateLaboratoryId);
            await OnInitializedAsync();
        }

        private void ViewLaboratory(Guid laboratoryId)
        {
            updateLaboratoryId = laboratoryId;
            viewLaboratorySectionIsActive = true;
            LaboratoryToView = Laboratories.FirstOrDefault(l => l.Id == updateLaboratoryId);
        }

        private void CloseView()
        {
            viewLaboratorySectionIsActive = false;
        }

        private async Task AddStudentsToLaboratory()
        {
            await LaboratoryDataService.AddStudentsToLaboratory(updateLaboratoryId, new List<Guid> { updateStudentId });
            await OnInitializedAsync();
        }

        private async Task AddAnnouncementToLaboratory()
        {
            var announcementInput = new AnnouncementInput
            {
                Header = announcementHeader,
                ProfessorId = announcementProfessorId,
                Text = announcementText
            };
            await LaboratoryDataService.AddAnnouncementsToLaboratory(updateLaboratoryId, new List<AnnouncementInput> { announcementInput });
            await OnInitializedAsync();
        }

        private async Task RemoveAnnouncementFromLaboratory()
        {
            await LaboratoryDataService.RemoveAnnouncementsFromLaboratory(updateLaboratoryId, new List<Guid> { removeAnnouncementId });
            await OnInitializedAsync();
        }

        private async Task AddGradingToLaboratory()
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
            await LaboratoryDataService.AddGradingsToLaboratory(updateLaboratoryId, new List<GradingInput> { gradingInput });
            await OnInitializedAsync();
        }

        private async Task RemoveGradingFromLaboratory()
        {
            await LaboratoryDataService.RemoveGradingsFromLaboratory(updateLaboratoryId, new List<Guid> { removeGradingId });
            await OnInitializedAsync();
        }
    }
}
