using LabsCourseManagement.Shared.Domain;
using Microsoft.AspNetCore.Components;
using LabsCourseManagement.UI.Pages.Services;

namespace LabsCourseManagement.UI.Pages
{
    public partial class ProfessorsOverview : ComponentBase
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; } = default!;
        [Inject]
        public IProfDataService ProfDataService { get; set; } = default!;
        public ProfessorCreateModel NewProfessor { get; set; } = new ProfessorCreateModel();
        public List<ProfessorModel> Professors { get; set; } = new List<ProfessorModel>();
        public List<ContactModel> Contacts { get; set; } = new List<ContactModel>();
        private Guid GuidProfessorForUpdate;
        private Guid GuidConatctForUpdate;
        private string PhoneNumber = default!;
        private Guid CourseGuid;
        private List<CourseModel> Courses = new List<CourseModel>();
        private string Name;
        private string Surname;

        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            foreach (var contactInfo in Professors.Select(prof => prof.ContactInfo))
            {
                if (contactInfo != null)
                {
                    var contact = new ContactModel(contactInfo.Id);
                    if (Contacts.Contains(contact)==false){
                        Contacts.Add(contact);
                    }
                }
            }

        }
        private async Task CreateProfessor()
        {
            await ProfDataService.CreateProfessor(NewProfessor);
            await OnInitializedAsync();
        }
        private async Task DeleteProfessor(Guid profId)
        {
            await ProfDataService.DeleteProfessor(profId);
            await OnInitializedAsync();

        }
        private async Task UpdateProfessor()
        {
            await ProfDataService.UpdateProfessorPhoneNumber(GuidProfessorForUpdate, GuidConatctForUpdate, PhoneNumber);
            await OnInitializedAsync();
        }
        private async Task AddCourses()
        {
            await ProfDataService.AddCourse(CourseGuid, GuidProfessorForUpdate);
            await OnInitializedAsync();
        }
        private async Task UpdateProfessorName()
        {
            await ProfDataService.UpdateName(Name, GuidProfessorForUpdate);
            await OnInitializedAsync();
        }
        private async Task UpdateProfessorSurname()
        {
            await ProfDataService.UpdateSurname(Surname, GuidProfessorForUpdate);
            await OnInitializedAsync();
        }
    }
    public class ProfessorCreateModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

