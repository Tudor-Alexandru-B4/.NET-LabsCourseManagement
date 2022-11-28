using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LabsCourseManagement.IntegrationTests
{
    public class CatalogsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/catalogs";

        [Fact]
        public async void When_CreatingCatalog_Then_ShouldReturnGatalogInGetRequest()
        {
            //Arrange
            var courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCatalogResult = await HttpClientCatalogs.GetAsync(ApiUrl);

            //Assert
            getCatalogResult.EnsureSuccessStatusCode();
            var catalogs = await getCatalogResult.Content.ReadFromJsonAsync<List<CatalogDto>>();

            catalogs.Should().NotBeNull();
            catalogs.Should().NotBeEmpty();
            catalogs.Count.Should().Be(1);
        }

        private async Task<CreateCourseDto> SUT()
        {
            var professorDto = new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "ProfessorPhoneNumber"
            };
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            return new CreateCourseDto()
            {
                Name = "CourseName",
                ProfessorId = professors[0].Id
            };
        }
    }
}
