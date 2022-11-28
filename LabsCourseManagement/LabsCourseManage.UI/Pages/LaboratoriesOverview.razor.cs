using LabsCourseManage.UI.Pages.Services;
using LabsCourseManagement.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManage.UI.Pages
{
    public partial class LaboratoriesOverview : ComponentBase
    {
       [Inject]
        public ILaboratoryDataService LaboratoryDataService { get; set; }

        public List<Laboratory> Laboratories { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            Laboratories = (await LaboratoryDataService.GetAllLaboratories()).ToList();
        }
    }
}
