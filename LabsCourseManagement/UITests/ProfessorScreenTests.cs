using Bunit;
using LabsCourseManagement.UI.Pages;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UITests
{
    public class ProfessorScreenTests
    {
        
        [Fact]
        public async Task ProfessorShouldBeAddedWhenAddButtonIsClickedAsync()
        {
            using var context = new TestContext();
            context.Services.AddHttpClient<IProfDataService,ProfDataService>();
            context.Services.AddHttpClient<ICourseDataService, CourseDataService>();
            var component = context.RenderComponent<ProfessorsOverview>();

            component.Find("input[id=name]").Change("Matei");
            component.Find("input[id=surname]").Change("Ioan");
            component.Find("input[id=phoneNumber]").Change("0712312312");

            var form = component.Find("div>section>form");
            await form.SubmitAsync();

            var cells = component.FindAll("tabel>tbody>tr");
            var number = cells.Count;

            Assert.Equal(1, number);
        }
    }
}