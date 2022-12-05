using LabsCourseManagement.Shared.Domain;
using Microsoft.AspNetCore.Components;
using LabsCourseManagement.UI.Pages.Services;

namespace LabsCourseManagement.UI.Pages
{
    public partial class ProfessorsOverview : ComponentBase
    {
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
        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors()).ToList();
            foreach (var prof in Professors)
            {
                var contact = new ContactModel(prof.ContactInfo.Id);
                Contacts.Add(contact);
            }

        }
        private async void CreateProfessor()
        {
            await ProfDataService.CreateProfessor(NewProfessor);
        }
        private async void DeleteProfessor()
        {

            foreach (var professor in Professors)
            {
                Guids.Add(professor.Id);
            }
            await ProfDataService.DeleteProfessor(GuidForDelete);

        }
        private async void UpdateProfessor()
        {

            await ProfDataService.UpdateProfessorPhoneNumber(GuidProfessorForUpdate, GuidConatctForUpdate, PhoneNumber);
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

