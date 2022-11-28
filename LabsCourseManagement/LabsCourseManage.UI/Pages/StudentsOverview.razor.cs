using LabsCourseManage.UI.Pages.Services;
using LabsCourseManagement.Shared;
using Microsoft.AspNetCore.Components;

namespace LabsCourseManage.UI.Pages
{
    public partial class StudentsOverview : ComponentBase
    {
        [Inject]
        public IStudentDataService StudentDataService { get; set; }

        public List<Student> Students { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            Students = (await StudentDataService.GetAllStudent()).ToList();
        }
    }
}
