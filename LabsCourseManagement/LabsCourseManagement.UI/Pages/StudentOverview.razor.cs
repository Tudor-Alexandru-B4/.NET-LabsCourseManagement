using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class StudentOverview : ComponentBase
    {
        [Inject]
        public IStudentDataService StudentDataService { get; set; }
        public StudentCreateModel NewStudent = new StudentCreateModel();
        public List<StudentModel> Students { get; set; } = default!;
        List<Guid> Guids = new List<Guid>();
        Guid GuidForDelete;
        Guid GuidForChangeGroup;
        string Group;
        protected override async Task OnInitializedAsync()
        {
            Students = (await StudentDataService.GetAllStudent()).ToList();
        }
        private async Task CreateStudent()
        {
            await StudentDataService.CreateStudent(NewStudent);   
        }
        private async Task DeleteStudent()
        {
            foreach(var student in Students)
            {
                Guids.Add(student.StudentId);
            }
            await StudentDataService.DeleteStudent(GuidForDelete);
        }

        private async Task UpdateStudent()
        {
            await StudentDataService.UpdateStudentGroup(GuidForChangeGroup, Group);
        }
    }

    public class StudentCreateModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }
        public string Group { get; set; }
        public bool IsActive { get; set; }
        public string RegistrationNumber { get; set; }
        public StudentCreateModel() { }

    }
}
