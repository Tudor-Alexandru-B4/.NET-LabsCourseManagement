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

        private List<Guid> Guids = new List<Guid>();
        private Guid GuidForDelete;
        private Guid GuidProfessorForUpdate;
        private Guid GuidConatctForUpdate;
        private string PhoneNumber = default!;
        private Guid CourseGuid;
        private List<CourseModel> Courses = new List<CourseModel>();

        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            foreach (var contactInfo in Professors.Select(prof => prof.ContactInfo))
            {
                if (contactInfo != null)
                {
                    var contact = new ContactModel(contactInfo.Id);
                    Contacts.Add(contact);
                }
            }
        }
        private async Task CreateProfessor()
        {
            await ProfDataService.CreateProfessor(NewProfessor);
            await OnInitializedAsync();
        }
        private async Task DeleteProfessor()
        {

            foreach (var professor in Professors)
            {
                Guids.Add(professor.Id);
            }
            await ProfDataService.DeleteProfessor(GuidForDelete);
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

    }
    public class ProfessorCreateModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

