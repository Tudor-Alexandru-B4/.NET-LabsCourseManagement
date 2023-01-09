using System;
using System.Threading.Tasks;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class InformationStringsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v2/api/informationstrings";

        [Fact]
        public async void When_GetInformationStringById_Then_ShouldGetInformationString()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"v2/api/courses/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var getInformationStringResponse =
                await HttpClientInformationStrings.GetAsync($"{ApiUrl}/{coursesAfterAdd[0].HelpfulMaterials[0].Id}");
            var informationString =
                await getInformationStringResponse.Content.ReadFromJsonAsync<InformationStringDto>();

            //Assert
            getInformationStringResponse.EnsureSuccessStatusCode();
            getInformationStringResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            informationString.String.Should().Be("material");
        }

        [Fact]
        public async void When_GetInformationStringByNonexistentId_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"v2/api/courses/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var getInformationStringResponse =
                await HttpClientInformationStrings.GetAsync($"{ApiUrl}/{Guid.NewGuid()}");
            var informationString =
                await getInformationStringResponse.Content.ReadFromJsonAsync<InformationStringDto>();

            //Assert
            getInformationStringResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_GetInformationString_Then_ShouldGetInformationString()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"v2/api/courses/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var getInformationStringResponse =
                await HttpClientInformationStrings.GetAsync($"{ApiUrl}");
            var informationString =
                await getInformationStringResponse.Content.ReadFromJsonAsync<List<InformationStringDto>>();

            //Assert
            getInformationStringResponse.EnsureSuccessStatusCode();
            getInformationStringResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            informationString[0].String.Should().Be("material");
        }

        private async Task<CreateCourseDto> SUT()
        {
            var professorDto = new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "0799779036"
            };
            await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
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
