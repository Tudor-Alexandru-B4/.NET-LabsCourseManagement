using System;
using System.Linq;
using System.Threading.Tasks;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class CoursesControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/courses";

        [Fact]
        public async void When_CreateCourse_Then_ShouldReturnCourseInGetRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResult = await HttpClientCourses.GetAsync(ApiUrl);

            //Assert
            createCourseResponse.EnsureSuccessStatusCode();
            createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getCourseResult.EnsureSuccessStatusCode();
            var courses = await getCourseResult.Content.ReadFromJsonAsync<List<CourseDto>>();
            courses.Should().NotBeNull();
            courses.Should().NotBeEmpty();
            courses.Count.Should().Be(1);
        }

        [Fact]
        public async void When_CreateCourse_Then_ProfessorsShouldNotBeEmpty()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResult = await HttpClientCourses.GetAsync(ApiUrl);

            //Assert
            createCourseResponse.EnsureSuccessStatusCode();
            createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getCourseResult.EnsureSuccessStatusCode();
            var courses = await getCourseResult.Content.ReadFromJsonAsync<List<CourseDto>>();
            courses[0].Professors.Should().NotBeEmpty();
            courses[0].Professors.Count.Should().Be(1);
            courses[0].Professors[0].Id.Should().Be(courseDto.ProfessorId);
        }

        [Fact]
        public async void When_CreateCourseWithNullName_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            courseDto.Name = null;

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Assert
            createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_CreateCourseWithNonexistentProfessorId_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            courseDto.ProfessorId = Guid.NewGuid();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Assert
            createCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_DeleteCourse_Then_ShouldNotReturnCourseInGetRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var deleteCourseResponse = await HttpClientCourses.DeleteAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");
            var getCourseAfterDeleteResponse = await HttpClientCourses.GetAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");

            //Assert
            deleteCourseResponse.EnsureSuccessStatusCode();
            deleteCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getCourseAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_DeleteNonexistantCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange

            //Act
            var deleteCourseResponse = await HttpClientCourses.DeleteAsync($"{ApiUrl}/{Guid.NewGuid()}");

            //Assert
            deleteCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddProfessorsToCourse_Then_ShouldBeAdded()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            var professorDto = new CreateProfessorDto()
            {
                Name = "AddedProfessorName",
                Surname = "AddedProfessorSurname",
                PhoneNumber = "0799779999"
            };
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            ProfessorDto professor = professors.FirstOrDefault(p => p.Name == "AddedProfessorName");
            var addProfessorResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/professors", new List<Guid>
            {
                professor.Id
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            
            addProfessorResponse.EnsureSuccessStatusCode();
            addProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            ProfessorDto professorAdded = null;
            foreach (var prof in coursesAfterAdd[coursesAfterAdd.Count - 1].Professors)
            {
                if (prof.Id == professor.Id)
                {
                    professorAdded = prof;
                    break;
                }
            }

            coursesAfterAdd[coursesAfterAdd.Count - 1].Professors.Count.Should().Be(2);
            professorAdded.Id.Should().Be(professor.Id);
            professorAdded.Name.Should().Be(professor.Name);
            professorAdded.Surname.Should().Be(professor.Surname);
        }

        [Fact]
        public async void When_AddProfessorsToNonExistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            var professorDto = new CreateProfessorDto()
            {
                Name = "AddedProfessorName",
                Surname = "AddedProfessorSurname",
                PhoneNumber = "0799779999"
            };
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            ProfessorDto professor = professors.FirstOrDefault(p => p.Name == "AddedProfessorName");
            var addProfessorResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{Guid.NewGuid()}/professors", new List<Guid>
            {
                professor.Id
            });

            //Assert

            addProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddEmptyProfessorsListToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var addProfessorResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/professors", new List<Guid>());

            //Assert

            addProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddNonexistentProfessorsListToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var addProfessorResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/professors", new List<Guid>
            {
                Guid.NewGuid()
            });

            //Assert

            addProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddStudentsToCourse_Then_ShouldBeAdded()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            var studentDto = new CreateStudentDto
            {
                Name = "AddedStudentName",
                Surname = "AddedStudentSurname",
                Year = 1,
                Group = "B4",
                RegistrationNumber = "123456789",
                PhoneNumber = "0721586412"
            };
            await HttpClientStudents.PostAsJsonAsync("v1/api/students", studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync("v1/api/students");
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var addStudentResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/students", new List<Guid>
            {
                students[0].StudentId
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert

            addStudentResponse.EnsureSuccessStatusCode();
            addStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[coursesAfterAdd.Count - 1].Students.Count.Should().Be(1);
            coursesAfterAdd[coursesAfterAdd.Count - 1].Students[0].StudentId.Should().Be(students[students.Count - 1].StudentId);
            coursesAfterAdd[coursesAfterAdd.Count - 1].Students[0].Name.Should().Be(students[students.Count - 1].Name);
            coursesAfterAdd[coursesAfterAdd.Count - 1].Students[0].Surname.Should().Be(students[students.Count - 1].Surname);
        }

        [Fact]
        public async void When_AddStudentsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            var studentDto = new CreateStudentDto
            {
                Name = "AddedStudentName",
                Surname = "AddedStudentSurname",
                Year = 1,
                Group = "B4",
                RegistrationNumber = "123456789",
                PhoneNumber = "0721586412"
            };
            await HttpClientStudents.PostAsJsonAsync("v1/api/students", studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync("v1/api/students");
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();

            //Act
            var addStudentResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{Guid.NewGuid()}/students", new List<Guid>
            {
                students[0].StudentId
            });

            //Assert

            addStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddStudentsEmptyListToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var addStudentResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/students", new List<Guid>());

            //Assert
            addStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddNonexistantStudentsToCourse_Then_ShouldBeAdded()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var addStudentResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}/students", new List<Guid>
            {
                Guid.NewGuid()
            });

            //Assert
            addStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_GetByIdCourse_Then_ShouldGetCourse()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            var getCourseByIdResponse = await HttpClientCourses.GetAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");

            //Assert
            getCourseByIdResponse.EnsureSuccessStatusCode();
            getCourseByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var course = await getCourseByIdResponse.Content.ReadFromJsonAsync<CourseDto>();

            course.Should().NotBeNull();
            course.Id.Should().Be(courses[courses.Count - 1].Id);
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
