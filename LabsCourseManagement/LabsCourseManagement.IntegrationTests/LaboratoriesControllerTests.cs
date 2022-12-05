using System;
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
            //laboratories.Count.Should().Be(1);
        }

        [Fact]
        public async void When_DeleteLaboratory_Then_ShouldNotReturnLaboratoryInGetRequest()
        {
            //Arrange
            CreateLaboratoryDto laboratoryDto = await SUT();

            //Act
            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync(ApiUrl, laboratoryDto);
            var getLaboratoryResponse = await HttpClientLaboratories.GetAsync(ApiUrl);
            var laboratories = await getLaboratoryResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();
            var deleteLaboratoryResponse = await HttpClientLaboratories.DeleteAsync($"{ApiUrl}/{laboratories[laboratories.Count - 1].Id}");
            var getLaboratoryAfterDeleteResponse = await HttpClientLaboratories.GetAsync($"{ApiUrl}/{laboratories[laboratories.Count - 1].Id}");

            //Assert
            deleteLaboratoryResponse.EnsureSuccessStatusCode();
            deleteLaboratoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getLaboratoryAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddStudentsToLaboratory_Then_ShouldBeAdded()
        {
            //Arrange
            CreateLaboratoryDto laboratoryDto = await SUT();
            var studentDto = new CreateStudentDto
            {
                Name = "AddedStudentName",
                Surname = "AddedStudentSurname",
                Year = 1,
                Group = "AddedStudentGroup",
                RegistrationNumber = "AddedStudentRegistrationNumber",
                PhoneNumber = "AddedStudentPhoneNumber"
            };
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync("v1/api/students", studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync("v1/api/students");
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();

            //Act
            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync(ApiUrl, laboratoryDto);
            var getLaboratoryResponse = await HttpClientLaboratories.GetAsync(ApiUrl);
            var laboratories = await getLaboratoryResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();
            var addStudentResponse = await HttpClientLaboratories.PostAsJsonAsync($"{ApiUrl}/{laboratories[laboratories.Count - 1].Id}/addStudents", new List<Guid>
            {
                students[0].StudentId
            });

            var getLaboratoryAfterAddResponse = await HttpClientLaboratories.GetAsync(ApiUrl);
            var laboratoriesAfterAdd = await getLaboratoryAfterAddResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();

            //Assert

            addStudentResponse.EnsureSuccessStatusCode();
            addStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            laboratoriesAfterAdd[laboratoriesAfterAdd.Count - 1].LaboratoryStudents.Count.Should().Be(1);
            laboratoriesAfterAdd[laboratoriesAfterAdd.Count - 1].LaboratoryStudents[0].StudentId.Should().Be(students[students.Count - 1].StudentId);
            laboratoriesAfterAdd[laboratoriesAfterAdd.Count - 1].LaboratoryStudents[0].Name.Should().Be(students[students.Count - 1].Name);
            laboratoriesAfterAdd[laboratoriesAfterAdd.Count - 1].LaboratoryStudents[0].Surname.Should().Be(students[students.Count - 1].Surname);
        }

        [Fact]
        public async void When_GetByIdLaboratory_Then_ShouldGetLaboratory()
        {
            //Arrange
            CreateLaboratoryDto laboratoryDto = await SUT();

            //Act
            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync(ApiUrl, laboratoryDto);
            var getLaboratoryResponse = await HttpClientLaboratories.GetAsync(ApiUrl);
            var laboratories = await getLaboratoryResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();
            var getLaboratoryByIdResponse = await HttpClientLaboratories.GetAsync($"{ApiUrl}/{laboratories[laboratories.Count - 1].Id}");

            //Assert
            getLaboratoryByIdResponse.EnsureSuccessStatusCode();
            getLaboratoryByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var laboratory = await getLaboratoryByIdResponse.Content.ReadFromJsonAsync<LaboratoryDto>();

            laboratory.Should().NotBeNull();
            laboratory.Id.Should().Be(laboratories[laboratories.Count - 1].Id);
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
