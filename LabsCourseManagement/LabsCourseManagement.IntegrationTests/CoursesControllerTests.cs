using System;
using System.Linq;
using System.Threading.Tasks;
using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class CoursesControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/courses";
        private const string ApiUrl2 = "v2/api/courses";

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
        public async void When_DeleteCourse_Then_ShouldAlsoDeleteLabs()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var labDto = LabSUT(courses[courses.Count - 1].Id);

            //Act
            var labAddResponse = await HttpClientLaboratories.PostAsJsonAsync("v1/api/laboratories", labDto);

            var deleteCourseResponse = await HttpClientCourses.DeleteAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");
            var getCourseAfterDeleteResponse = await HttpClientCourses.GetAsync($"{ApiUrl}/{courses[courses.Count - 1].Id}");

            var getLabAfterDeleteResponse = await HttpClientLaboratories.GetAsync("v1/api/laboratories");
            
            //Assert
            deleteCourseResponse.EnsureSuccessStatusCode();
            deleteCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getCourseAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            var labsAfterDelete = await getLabAfterDeleteResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();
            labsAfterDelete.Count.Should().Be(0);
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
        public async void When_RemoveProfessorsToCourse_Then_ShouldBeRemoved()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProfessorResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/professors",
                new List<Guid>()
                {
                    professors[0].Id
                });

            var getCourseAfterRemoveResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterRemove = await getCourseAfterRemoveResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert

            removeProfessorResponse.EnsureSuccessStatusCode();
            removeProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterRemove[0].Professors.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveProfessorsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var removeProfessorResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/professors",
                new List<Guid>()
                {
                    professors[0].Id
                });

            //Assert

            removeProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentProfessorsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProfessorResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/professors",
                new List<Guid>()
                {
                    Guid.NewGuid()
                });

            //Assert

            removeProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyProfessorsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProfessorResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/professors", new List<Guid>());

            //Assert

            removeProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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
        public async void When_RemoveStudentsToCourse_Then_ShouldBeRemoved()
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

            var deleteResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/students",
                new List<Guid>()
                {
                    students[0].StudentId
                });

            var getCourseAfterDeleteResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterDeleteResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert

            deleteResponse.EnsureSuccessStatusCode();
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[0].Students.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveStudentsToNonexistentCourse_Then_ShouldReturnBadRequest()
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

            var deleteResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/students",
                new List<Guid>()
                {
                    students[0].StudentId
                });

            //Assert

            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentStudentsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var deleteResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/students",
                new List<Guid>()
                {
                    Guid.NewGuid()
                });

            //Assert
            
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyStudentsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var deleteResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/students", new List<Guid>());

            var getCourseAfterDeleteResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterDeleteResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

        [Fact]
        public async void When_UpdateNameCourse_Then_ShouldUpodateName()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            string newName = "NewCourseName";

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var updateNameResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/name", newName);

            var getCourseAfterUpdateResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterUpdate = await getCourseAfterUpdateResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            //Assert

            updateNameResponse.EnsureSuccessStatusCode();
            updateNameResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterUpdate[0].Name.Should().Be(newName);
        }

        [Fact]
        public async void When_UpdateNameNonexistentCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            string newName = "NewCourseName";

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var updateNameResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/name", newName);
            
            //Assert
            updateNameResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_UpdateNullNameCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            string newName = null;

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var updateNameResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/name", newName);

            //Assert
            updateNameResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_UpdateActiveStatusCourse_Then_ShouldUpodateActiveStatus()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            bool newActiveStatus = false;

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var updateActiveResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/active", newActiveStatus);

            var getCourseAfterUpdateResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterUpdate = await getCourseAfterUpdateResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            //Assert

            updateActiveResponse.EnsureSuccessStatusCode();
            updateActiveResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterUpdate[0].IsActive.Should().Be(newActiveStatus);
        }

        [Fact]
        public async void When_UpdateActiveStatusNonexistentCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            bool newActiveStatus = false;

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var updateActiveResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/active", newActiveStatus);

            //Assert
            updateActiveResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_UpdateNullActiveStatusCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            string activeStatus = null;

            //Act
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var updateActiveResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/active", activeStatus);

            //Assert
            updateActiveResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddAnnouncementsToCourse_Then_ShouldAddAnnouncements()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();
            
            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            addAnnouncementResponse.EnsureSuccessStatusCode();
            addAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[0].CourseAnnouncements.Count.Should().Be(1);
            coursesAfterAdd[0].CourseAnnouncements[0].Header.Should().Be("Header");
            coursesAfterAdd[0].CourseAnnouncements[0].Text.Should().Be("Text");
            coursesAfterAdd[0].CourseAnnouncements[0].Writer.Id.Should().Be(professors[0].Id);
        }

        [Fact]
        public async void When_AddAnnouncementsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            //Assert
            addAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddEmptyAnnouncementsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>());

            //Assert
            addAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddAnnouncementsWithNonexistentWriterToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = Guid.NewGuid()
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            //Assert
            addAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddAnnouncementsWithNullElementsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = null,
                Text = null,
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            //Assert
            addAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_RemoveAnnouncementsToCourse_Then_ShouldRemoveAnnouncements()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeAnnouncementsResponse = await HttpClientCourses.PutAsJsonAsync(
                $"{ApiUrl2}/{courses[0].Id}/announcements", new List<Guid>()
                {
                    coursesAfterAdd[0].CourseAnnouncements[0].Id
                });

            var getCourseAfterRemoveResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterRemove = await getCourseAfterRemoveResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            removeAnnouncementsResponse.EnsureSuccessStatusCode();
            removeAnnouncementsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterRemove[0].CourseAnnouncements.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveAnnouncementsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeAnnouncementsResponse = await HttpClientCourses.PutAsJsonAsync(
                $"{ApiUrl2}/{Guid.NewGuid()}/announcements", new List<Guid>()
                {
                    coursesAfterAdd[0].CourseAnnouncements[0].Id
                });

            //Assert
            removeAnnouncementsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentAnnouncementsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeAnnouncementsResponse = await HttpClientCourses.PutAsJsonAsync(
                $"{ApiUrl2}/{courses[0].Id}/announcements", new List<Guid>()
                {
                    Guid.NewGuid()
                });

            //Assert
            removeAnnouncementsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyAnnouncementsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var announcementDto = new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text",
                ProfessorId = professors[0].Id
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addAnnouncementResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/announcements", new List<CreateAnnouncementDto>()
            {
                announcementDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeAnnouncementsResponse = await HttpClientCourses.PutAsJsonAsync(
                $"{ApiUrl2}/{courses[0].Id}/announcements", new List<Guid>());

            //Assert
            removeAnnouncementsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

        [Fact]
        public async void When_AddProgramsToCourse_Then_ShouldAddPrograms()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            addProgramsResponse.EnsureSuccessStatusCode();
            addProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[0].CourseProgram.Count.Should().Be(1);
            coursesAfterAdd[0].CourseProgram[0].Classroom.Should().Be("C2");
            coursesAfterAdd[0].CourseProgram[0].DateAndTime.Should().Be(DateTime.Parse("09.01.2021 20:30"));
        }

        [Fact]
        public async void When_AddProgramsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            //Assert
            addProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddEmptyProgramsTotCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>());

            //Assert
            addProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddProgramsWithNullElementsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = null,
                DateTime = null
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            //Assert
            addProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddExistingProgramsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var addAddedProgramsResponse = await HttpClientCourses.PostAsJsonAsync(
                $"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
                {
                    programDto
                });

            //Assert
            addAddedProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_RemoveProgramsToCourse_Then_ShouldAddRemove()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProgramsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs",
                new List<Guid>()
                {
                    coursesAfterAdd[0].CourseProgram[0].Id
                });

            var getCourseAfterRemoveResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterRemove = await getCourseAfterRemoveResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            removeProgramsResponse.EnsureSuccessStatusCode();
            removeProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterRemove[0].CourseProgram.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveProgramsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProgramsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/programs",
                new List<Guid>()
                {
                    coursesAfterAdd[0].CourseProgram[0].Id
                });

            //Assert
            removeProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentProgramsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProgramsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs",
                new List<Guid>()
                {
                    Guid.NewGuid()
                });

            //Assert
            removeProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyProgramsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var programDto = new CreateTimeAndPlaceDto()
            {
                Classroom = "C2",
                DateTime = "09.01.2021 20:30"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addProgramsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs", new List<CreateTimeAndPlaceDto>()
            {
                programDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeProgramsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/programs",
                new List<Guid>());

            //Assert
            removeProgramsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async void When_AddGradingsToCourse_Then_ShouldAddGradings()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            addGradingsResponse.EnsureSuccessStatusCode();
            addGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[0].CourseGradingInfo.Count.Should().Be(1);
            coursesAfterAdd[0].CourseGradingInfo[0].ExaminationType.Should().Be(ExaminationType.Other);
            coursesAfterAdd[0].CourseGradingInfo[0].IsMandatory.Should().Be(true);
            coursesAfterAdd[0].CourseGradingInfo[0].MinGrade.Should().Be(1);
            coursesAfterAdd[0].CourseGradingInfo[0].MaxGrade.Should().Be(9);
            coursesAfterAdd[0].CourseGradingInfo[0].Description.Should().Be("course exam");
        }

        [Fact]
        public async void When_AddGradingsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            //Assert
            addGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddEmptyGradingsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>());

            //Assert
            addGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddGradingsWithNullElementsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = null,
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = null,
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            //Assert
            addGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddGradingsWithWrongExaminationTypeToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "any",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            //Assert
            addGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddExistingGradingsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var addAddedGradingsResponse = await HttpClientCourses.PostAsJsonAsync(
                $"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
                {
                    gradingDto
                });

            //Assert
            addAddedGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_RemoveGradingsToCourse_Then_ShouldRemoveGradings()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeGradingsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings",
                new List<Guid>()
                {
                    coursesAfterAdd[0].CourseGradingInfo[0].Id
                });

            var getCourseAfterRemoveResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterRemove = await getCourseAfterRemoveResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            removeGradingsResponse.EnsureSuccessStatusCode();
            removeGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterRemove[0].CourseGradingInfo.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveGradingsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeGradingsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/gradings",
                new List<Guid>()
                {
                    coursesAfterAdd[0].CourseGradingInfo[0].Id
                });

            //Assert
            removeGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentGradingsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeGradingsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings",
                new List<Guid>()
                {
                    Guid.NewGuid()
                });

            //Assert
            removeGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyGradingsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            var gradingDto = new CreateGradingsDto()
            {
                ExaminationType = "other",
                IsMandatory = true,
                MinGrade = 1,
                MaxGrade = 9,
                Description = "course exam",
                Classroom = "C2",
                DateTime = "19.10.2015 20:55"
            };

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addGradingsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings", new List<CreateGradingsDto>()
            {
                gradingDto
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeGradingsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/gradings",
                new List<Guid>());

            //Assert
            removeGradingsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_AddMaterialsToCourse_Then_ShouldAddMaterials()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            addMaterialsResponse.EnsureSuccessStatusCode();
            addMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterAdd[0].HelpfulMaterials.Count.Should().Be(1);
            coursesAfterAdd[0].HelpfulMaterials[0].String.Should().Be("material");
        }

        [Fact]
        public async void When_AddMaterialsToNonexistentCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/materials", new List<string>()
            {
                "material"
            });

            //Assert
            addMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddEmptyMaterialsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>());

            //Assert
            addMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_RemoveMaterialsToCourse_Then_ShouldRemoveMaterials()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeMaterialsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<Guid>()
            {
                coursesAfterAdd[0].HelpfulMaterials[0].Id
            });

            var getCourseAfterRemoveResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterRemove = await getCourseAfterRemoveResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Assert
            removeMaterialsResponse.EnsureSuccessStatusCode();
            removeMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            coursesAfterRemove[0].HelpfulMaterials.Count.Should().Be(0);
        }

        [Fact]
        public async void When_RemoveMaterialsToNonexistentCourse_Then_ShouldReturnNotFount()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeMaterialsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{Guid.NewGuid()}/materials", new List<Guid>()
            {
                coursesAfterAdd[0].HelpfulMaterials[0].Id
            });

            //Assert
            removeMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveNonexistentMaterialsToCourse_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeMaterialsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<Guid>()
            {
                Guid.NewGuid()
            });

            //Assert
            removeMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_RemoveEmptyMaterialsToCourse_Then_ShouldReturnBadRequest()
        {
            CleanDatabases();
            //Arrange
            CreateCourseDto courseDto = await SUT();
            await HttpClientCourses.PostAsJsonAsync(ApiUrl, courseDto);

            //Act
            var getCourseResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var addMaterialsResponse = await HttpClientCourses.PostAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<string>()
            {
                "material"
            });

            var getCourseAfterAddResponse = await HttpClientCourses.GetAsync(ApiUrl);
            var coursesAfterAdd = await getCourseAfterAddResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var removeMaterialsResponse = await HttpClientCourses.PutAsJsonAsync($"{ApiUrl2}/{courses[0].Id}/materials", new List<Guid>());

            //Assert
            removeMaterialsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        private async Task<CreateLaboratoryDto> LabSUT(Guid courseId)
        {
            var professorDto = new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "0744556324"
            };
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            return new CreateLaboratoryDto()
            {
                Name = "LaboratoryName",
                CourseId = courseId,
                ProfessorId = professors[0].Id,
                DateTime = "20-11-22 3:55",
                Place = "C409"
            };
        }
    }
}
