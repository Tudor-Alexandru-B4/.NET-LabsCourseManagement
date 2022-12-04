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
        protected override async Task OnInitializedAsync()
        {
            Professors = (await ProfDataService.GetAllProfessors()).ToList();
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

    }
    public class ProfessorCreateModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public ProfessorCreateModel() { }
    }
}

