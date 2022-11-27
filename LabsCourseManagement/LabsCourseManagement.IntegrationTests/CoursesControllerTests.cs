//using System.Threading.Tasks;

//namespace LabsCourseManagement.IntegrationTests
//{
//    public class CoursesControllerTests : BaseIntegrationTests
//    {
//        private const string ApiUrl = "v1/api/courses";

//        [Fact]
//        public async void When_CreateCourse_Then_ShouldReturnCourseInGetRequest()
//        {
//            //Arrange
//            //CleanDatabases();
//            CreateCourseDto courseDto = await SUT();

//            //Act
//            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
//            var getCourseResult = await HttpClientCourses.GetAsync(ApiUrl);

//            //Assert
//            createCourseResponse.EnsureSuccessStatusCode();
//            createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

//            getCourseResult.EnsureSuccessStatusCode();
//            var courses = await getCourseResult.Content.ReadFromJsonAsync<List<Course>>();
//            courses.Should().NotBeNull();
//            courses.Count.Should().Be(1);
//        }

//        //[Fact]
//        //public async void When_CreateCourse_Then_ProfessorsShouldNotBeEmpty()
//        //{
//        //    //Arrange
//        //    CleanDatabases();
//        //    CreateCourseDto courseDto = SUT();

//        //    //Act
//        //    var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
//        //    var getCourseResult = await HttpClientCourses.GetAsync(ApiUrl);

//        //    //Assert
//        //    createCourseResponse.EnsureSuccessStatusCode();
//        //    createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

//        //    getCourseResult.EnsureSuccessStatusCode();
//        //    var courses = await getCourseResult.Content.ReadFromJsonAsync<List<Course>>();
//        //    courses[0].Professors.Should().NotBeEmpty();
//        //    courses[0].Professors.Count.Should().Be(1);
//        //    courses[0].Professors[0].Id.Should().Be(courseDto.ProfessorId);
//        //}

//        private async Task<CreateCourseDto> SUT()
//        {
//            var professorDto = new CreateProfessorDto()
//            {
//                Name = "ProfessorName",
//                Surname = "ProfessorSurname"
//            };
//            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
//            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
//            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<Professor>>();

//            return new CreateCourseDto()
//            {
//                Name = "CourseName",
//                ProfessorId = professors[0].Id
//            };
//        }

//        [Fact]
//        public async void When_DeleteCourse_Then_ShouldNotReturnCourseInGetRequest()
//        {
//            //Arrange
//            CreateCourseDto courseDto = new CreateCourseDto()
//            {
//                Name = CourseName
//            };

//            //Act
//            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
//            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
//            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<Course>>();
//            var deleteCourseResponse = await HttpClientCourses.DeleteAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");
//            var getCourseAfterDeleteResponse = await HttpClientCourses.GetAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");

//            //Assert
//            deleteCourseResponse.EnsureSuccessStatusCode();
//            deleteCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

//            getCourseAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
//        }
//    }
//}
