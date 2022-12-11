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
        public ProfessorCreateModel NewProfessor = new ProfessorCreateModel();
        public List<ProfessorModel> Professors { get; set; } = default!;
        List<Guid> Guids = new List<Guid>();
        Guid GuidForDelete;
        Guid GuidProfessorForUpdate;
        Guid GuidConatctForUpdate;
        public List<ContactModel> Contacts=new List<ContactModel>();
        string PhoneNumber = default!;
        Guid CourseGuid;
        List<CourseModel> Courses= new List<CourseModel>();

        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors() ?? new List<ProfessorModel>()).ToList();
            Courses = (await CourseDataService.GetAllCourses() ?? new List<CourseModel>()).ToList();
            foreach (var prof in Professors)
            {
                var contactInfo = prof.ContactInfo;
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
        }
        private async Task DeleteProfessor()
        {

            foreach (var professor in Professors)
            {
                Guids.Add(professor.Id);
            }
            await ProfDataService.DeleteProfessor(GuidForDelete);

        }
        private async Task UpdateProfessor()
        {

            await ProfDataService.UpdateProfessorPhoneNumber(GuidProfessorForUpdate, GuidConatctForUpdate, PhoneNumber);
        }
        private async Task AddCourses()
        {
            await ProfDataService.AddCourse(CourseGuid, GuidProfessorForUpdate);
        }

    }
    public class ProfessorCreateModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

