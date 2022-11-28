using System.Net.Security;
using System.Threading.Tasks;

namespace LabsCourseManagement.IntegrationTests
{
    public class LaboratoriesControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/laboratories";

        [Fact]
        public async void When_CreateLaboratory_Then_ShouldGetLaboratoryInGetRequest()
        {
            //Arrange
            var laboratoryDto = await SUT();

            //Act
            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync(ApiUrl, laboratoryDto);
            var getLaboratoryResult = await HttpClientLaboratories.GetAsync(ApiUrl);

            //Assert
            createLaboratoryResponse.EnsureSuccessStatusCode();
            createLaboratoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getLaboratoryResult.EnsureSuccessStatusCode();
            var laboratories = await getLaboratoryResult.Content.ReadFromJsonAsync<List<LaboratoryDto>>();

            laboratories.Should().NotBeNull();
            laboratories.Should().NotBeEmpty();
            laboratories.Count.Should().Be(1);
        }

        private async Task<CreateLaboratoryDto> SUT()
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

            var courseDto = new CreateCourseDto()
            {
                Name = "CourseName",
                ProfessorId = professors[0].Id
            };
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCourseResult = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResult.Content.ReadFromJsonAsync<List<CourseDto>>();

            return new CreateLaboratoryDto()
            {
                Name = "LaboratoryName",
                CourseId = courses[0].Id,
                ProfessorId = professors[0].Id,
                DateTime = "20-11-22 3:55",
                Place = "C409"
            };
        }
    }
}
