using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class StudentOverview : ComponentBase
    {
        [Inject]
        public NavigationManager uriHelper { get; set; } = default!;
        [Inject]
        public IStudentDataService StudentDataService { get; set; } = default!;
        public StudentCreateModel NewStudent { get; set; } = new StudentCreateModel();
        public List<StudentModel> Students { get; set; } = new List<StudentModel>();

        private List<Guid> Guids = new List<Guid>();
        private Guid GuidForDelete;
        private Guid GuidStudentForUpdate { get; set; }
        private string Group = default!;
        private string Year = default!;
        private string Name = default!;
        private string Surname = default!;
        private string RegistrationNumber = default!;

        protected override async Task OnInitializedAsync()
        {
            Students = (await StudentDataService.GetAllStudents() ?? new List<StudentModel>()).ToList();
        }
        private async Task CreateStudent()
        {
            await StudentDataService.CreateStudent(NewStudent);
            //uriHelper.NavigateTo(uriHelper.Uri, forceLoad: true);
            await OnInitializedAsync();
        }
        private async Task DeleteStudent(Guid id)
        {
            foreach(var student in Students)
            {
                Guids.Add(student.StudentId);
            }
            await StudentDataService.DeleteStudent(id);
            await OnInitializedAsync();
        }
        private async Task UpdateStudentName()
        {
            await StudentDataService.UpdateName(GuidStudentForUpdate, Name);
            await OnInitializedAsync();
        }
        private async Task UpdateStudentSurname()
        {
            await StudentDataService.UpdateSurname(GuidStudentForUpdate, Surname);
            await OnInitializedAsync();
        }
        private async Task UpdateStudentGroup()
        {
            await StudentDataService.UpdateGroup(GuidStudentForUpdate, Group);
            await OnInitializedAsync();
        }
        private async Task UpdateStudentYear()
        {
            int Year = Int32.Parse(this.Year);
            await StudentDataService.UpdateYear(GuidStudentForUpdate, Year);
            await OnInitializedAsync();
        }
        private async Task UpdateStudentRegistrationNumber()
        {
            await StudentDataService.UpdateRegistrationNumber(GuidStudentForUpdate, RegistrationNumber);
            await OnInitializedAsync();
        }


    }

    public class StudentCreateModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public bool IsActive { get; set; }
        public string? RegistrationNumber { get; set; }
    }
}
