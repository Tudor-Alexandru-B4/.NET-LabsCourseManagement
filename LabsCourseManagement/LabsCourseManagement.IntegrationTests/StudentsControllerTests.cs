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
            students.Count.Should().Be(1);
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
