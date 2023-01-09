using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

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
        private Guid GuidStudentForUpdate { get; set; }
        private string Group = default!;
        private string Year = default!;
        private string Name = default!;
        private string Surname = default!;
        private string RegistrationNumber = default!;
        private string Email = default!;

        protected override async Task OnInitializedAsync()
        {
            Students = (await StudentDataService.GetAllStudents() ?? new List<StudentModel>()).ToList();
        }
        private async Task CreateStudent()
        {
            await StudentDataService.CreateStudent(NewStudent);

            //Guid studentId = Guid.Empty;
            //Console.WriteLine(Students.ToList());
            //Console.WriteLine($"Nr matricol : {RegistrationNumber}");
            //foreach (var student in Students)
            //{
            //    if (student.RegistrationNumber == RegistrationNumber && student.Name==Name && student.Surname==Surname)
            //    {
            //        studentId = student.StudentId;
            //    }
            //}

            //await StudentDataService.AddEmail(studentId, Email);
            await OnInitializedAsync();


        }
        private async Task DeleteStudent(Guid id)
        {
            foreach (var student in Students)
            {
                Guids.Add(student.StudentId);
            }
            await StudentDataService.DeleteStudent(id);
            await OnInitializedAsync();
        }
        private async Task AddEmail()
        {
            await StudentDataService.AddEmail(GuidStudentForUpdate, Email);
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
        [Required(ErrorMessage = "Please insert a name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please insert a surname")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Please insert a phone number")]
        [RegularExpression(@"^(\+\d{1,3}( )?)?((\(\d{3}\))|\d{3})[- .]?\d{3}[- .]?\d{4}$", ErrorMessage = "Incorect format.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please insert a year that is greater than 0")]
        [Range(1, 10, ErrorMessage = "Number out of range: 1-10")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Please insert a group")]
        public string? Group { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please insert your registration number")]
        public string? RegistrationNumber { get; set; }

    }
}
