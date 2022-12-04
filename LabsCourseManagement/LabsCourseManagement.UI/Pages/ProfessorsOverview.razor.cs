using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManagement.UI.Pages
{
    public partial class ProfessorsOverview : ComponentBase
	{
		[Inject]
		public IProfDataService ProfDataService { get; set; }
		public List<ProfessorModel> Professors { get; set; } = default!;
		protected override async Task OnInitializedAsync()
		{
			Professors= (await ProfDataService.GetAllProfessors()).ToList();
		}

	}
}

