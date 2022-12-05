using System;

namespace LabsCourseManagement.IntegrationTests
{
    public class StudentsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/students";

        [Fact]
        public async void When_CreateStudent_Then_ShouldReturnStudentInGetRequest()
        {
            //Arrange
            var studentDto = SUT();

            //Act
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync(ApiUrl, studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync(ApiUrl);

            //Assert
            createStudentResponse.EnsureSuccessStatusCode();
            createStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getStudentResult.EnsureSuccessStatusCode();
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();

            students.Should().NotBeNull();
            students.Should().NotBeEmpty();
            //students.Count.Should().Be(1);
        }

        [Fact]
        public async void When_DeleteStudent_Then_ShouldNotReturnStudentInGetRequest()
        {
            //Arrange
            CreateStudentDto studentDto = SUT();

            //Act
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync(ApiUrl, studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync(ApiUrl);
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();
            var deleteStudentResponse = await HttpClientCourses.DeleteAsync($"{ApiUrl}/{students[students.Count - 1].StudentId}");
            var getStudentsAfterDeleteResponse = await HttpClientCourses.GetAsync($"{ApiUrl}/{students[students.Count - 1].StudentId}");

            //Assert
            deleteStudentResponse.EnsureSuccessStatusCode();
            deleteStudentResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getStudentsAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddCoursesToStudent_Then_ShouldBeAdded()
        {
            //Arrange
            CreateStudentDto studentDto = SUT();

            CreateProfessorDto professorDto = new CreateProfessorDto
            {
                Name = "Florin",
                Surname = "Olariu",
                PhoneNumber = "0733156778",
            };

            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var courseDto = new CreateCourseDto()
            {
                Name = "AddedProfessorName",
                ProfessorId = professors[professors.Count - 1].Id
            };

            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Act
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync(ApiUrl, studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync(ApiUrl);
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();
            var addCourseResponse = await HttpClientStudents.PostAsJsonAsync($"{ApiUrl}/{students[students.Count - 1].StudentId}/courses", new List<Guid>
            {
                courses[courses.Count - 1].Id
            });

            var getStudentsAfterAddResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var studentsAfterAdd = await getStudentsAfterAddResponse.Content.ReadFromJsonAsync<List<StudentDto>>();

            //Assert

            addCourseResponse.EnsureSuccessStatusCode();
            addCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            studentsAfterAdd[studentsAfterAdd.Count - 1].Courses.Count.Should().Be(1);
            studentsAfterAdd[studentsAfterAdd.Count - 1].Courses[0].Id.Should().Be(courses[courses.Count - 1].Id);
            studentsAfterAdd[studentsAfterAdd.Count - 1].Courses[0].Name.Should().Be(courses[courses.Count - 1].Name);
        }

        [Fact]
        public async void When_AddLaboratoriesToStudent_Then_ShouldBeAdded()
        {
            //Arrange
            CreateStudentDto studentDto = SUT();

            CreateProfessorDto professorDto = new CreateProfessorDto
            {
                Name = "Florin",
                Surname = "Olariu",
                PhoneNumber = "0733156778",
            };

            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var courseDto = new CreateCourseDto()
            {
                Name = "AddedProfessorName",
                ProfessorId = professors[professors.Count - 1].Id
            };

            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var laboratoryDto = new CreateLaboratoryDto()
            {
                Name = "LaboratoryName",
                CourseId = courses[0].Id,
                ProfessorId = professors[0].Id,
                DateTime = "20-11-22 3:55",
                Place = "C409"
            };

            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync("v1/api/laboratories", laboratoryDto);
            var getLaboratoryResponse = await HttpClientLaboratories.GetAsync("v1/api/laboratories");
            var laboratories = await getLaboratoryResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();

            //Act
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync(ApiUrl, studentDto);
            var getStudentResult = await HttpClientStudents.GetAsync(ApiUrl);
            var students = await getStudentResult.Content.ReadFromJsonAsync<List<StudentDto>>();
            var addLaboratoryResponse = await HttpClientStudents.PostAsJsonAsync($"{ApiUrl}/{students[students.Count - 1].StudentId}/laboratories", new List<Guid>
            {
                laboratories[laboratories.Count - 1].Id
            });
            var getStudentsAfterAddResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var studentsAfterAdd = await getStudentsAfterAddResponse.Content.ReadFromJsonAsync<List<StudentDto>>();

            //Assert

            addLaboratoryResponse.EnsureSuccessStatusCode();
            addLaboratoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            studentsAfterAdd[studentsAfterAdd.Count - 1].Laboratories.Count.Should().Be(1);
            studentsAfterAdd[studentsAfterAdd.Count - 1].Laboratories[0].Id.Should().Be(laboratories[laboratories.Count - 1].Id);
            studentsAfterAdd[studentsAfterAdd.Count - 1].Laboratories[0].Name.Should().Be(laboratories[laboratories.Count - 1].Name);
        }

        [Fact]
        public async void When_GetByIdStudent_Then_ShouldGetStudent()
        {
            //Arrange
            CreateStudentDto studentDto = SUT();

            //Act
            var createStudentResponse = await HttpClientStudents.PostAsJsonAsync(ApiUrl, studentDto);
            var getStudentResponse = await HttpClientStudents.GetAsync(ApiUrl);
            var students = await getStudentResponse.Content.ReadFromJsonAsync<List<StudentDto>>();
            var getStudentByIdResponse = await HttpClientStudents.GetAsync($"{ApiUrl}/{students[students.Count - 1].StudentId}");

            //Assert
            getStudentByIdResponse.EnsureSuccessStatusCode();
            getStudentByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var student = await getStudentByIdResponse.Content.ReadFromJsonAsync<StudentDto>();

            student.Should().NotBeNull();
            student.StudentId.Should().Be(students[students.Count - 1].StudentId);
        }

        private static CreateStudentDto SUT()
        {
            return new CreateStudentDto
            {
                Name = "StudentName",
                Surname = "StudentSurname",
                Year = 1,
                Group = "StudentGroup",
                RegistrationNumber = "StudentRegistrationNumber",
                PhoneNumber = "StudentPhoneNumber"
            };
        }
    }
}
