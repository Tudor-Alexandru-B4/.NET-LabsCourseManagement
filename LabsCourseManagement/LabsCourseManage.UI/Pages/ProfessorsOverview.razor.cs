using LabsCourseManage.UI.Pages.Services;
using LabsCourseManagement.Domain;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabsCourseManage.UI.Pages
{
	public partial class ProfessorsOverview : ComponentBase
	{
		[Inject]
		public IProfDataService ProfDataService { get; set; }
		public List<Professor> Professors { get; set; } = default!;
		protected override async Task OnInitializedAsync()
		{
			Professors= (await ProfDataService.GetAllProfessors()).ToList();
		}

	}
}

