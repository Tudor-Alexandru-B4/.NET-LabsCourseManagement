using System;
using System.Threading.Tasks;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class CatalogsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/catalogs";

        [Fact]
        public async void When_CreatingCatalog_Then_ShouldReturnGatalogInGetRequest()
        {
            CleanDatabases();
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

        [Fact]
        public async void When_GetByIdCatalog_Then_ShouldGetCatalog()
        {
            CleanDatabases();
            //Arrange
            var courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCatalogResult = await HttpClientCatalogs.GetAsync(ApiUrl);
            var catalogs = await getCatalogResult.Content.ReadFromJsonAsync<List<CatalogDto>>();
            var getCatalogByIdResponse = await HttpClientCatalogs.GetAsync($"{ApiUrl}/{catalogs[catalogs.Count - 1].Id}");

            //Assert
            getCatalogByIdResponse.EnsureSuccessStatusCode();
            getCatalogByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var catalog = await getCatalogByIdResponse.Content.ReadFromJsonAsync<CatalogDto>();

            catalog.Should().NotBeNull();
            catalog.Id.Should().Be(catalogs[catalogs.Count - 1].Id);
        }

        [Fact]
        public async void When_GetByIdNonexistantCatalog_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange

            //Act
            var getCatalogByIdResponse = await HttpClientCatalogs.GetAsync($"{ApiUrl}/{Guid.NewGuid()}");

            //Assert
            getCatalogByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task<CreateCourseDto> SUT()
        {
            var professorDto = new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "0799446257"
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
