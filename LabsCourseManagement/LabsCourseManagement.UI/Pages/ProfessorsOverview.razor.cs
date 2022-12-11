using LabsCourseManagement.Shared.Domain;
using Microsoft.AspNetCore.Components;
using LabsCourseManagement.UI.Pages.Services;

namespace LabsCourseManagement.UI.Pages
{
    public partial class ProfessorsOverview : ComponentBase
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }
        [Inject]
        public IProfDataService ProfDataService { get; set; }
        public ProfessorCreateModel NewProfessor = new ProfessorCreateModel();
        public List<ProfessorModel> Professors { get; set; } = default!;
        List<Guid> Guids = new List<Guid>();
        Guid GuidForDelete;
        Guid GuidProfessorForUpdate;
        Guid GuidConatctForUpdate;
        public List<ContactModel> Contacts=new List<ContactModel>();
        String PhoneNumber;
        Guid CourseGuid;
        List<CourseModel> Courses= new List<CourseModel>();

        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors()).ToList();
            Courses = (await CourseDataService.GetAllCourses()).ToList();
            foreach (var prof in Professors)
            {
                var contact = new ContactModel(prof.ContactInfo.Id);
                Contacts.Add(contact);
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
        private async void AddCourses()
        {
            await ProfDataService.AddCourse(CourseGuid, GuidProfessorForUpdate);
        }

    }
    public class ProfessorCreateModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public ProfessorCreateModel() { }
    }
}

