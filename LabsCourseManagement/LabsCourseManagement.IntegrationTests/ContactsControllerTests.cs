using System;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class ContactsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v2/api/contacts";

        [Fact]
        public async void When_GetByIdContact_Then_ShouldGetContact()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = ProfessorSUT();
            await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var getContactByIdResponse = await HttpClientContacts.GetAsync($"{ApiUrl}/{professors[0].ContactInfo.Id}");

            //Assert
            getContactByIdResponse.EnsureSuccessStatusCode();
            getContactByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var contact = await getContactByIdResponse.Content.ReadFromJsonAsync<ContactDto>();

            contact.Should().NotBeNull();
            contact.PhoneNumber.Should().Be("0799568741");
        }

        [Fact]
        public async void When_GetByIdNonexistentContact_Then_ShouldReturnNotFound()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = ProfessorSUT();
            await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var getContactByIdResponse = await HttpClientContacts.GetAsync($"{ApiUrl}/{Guid.NewGuid()}");

            //Assert
            getContactByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_GetContact_Then_ShouldGetContact()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = ProfessorSUT();
            await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var getContactByIdResponse = await HttpClientContacts.GetAsync($"{ApiUrl}");

            //Assert
            getContactByIdResponse.EnsureSuccessStatusCode();
            getContactByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var contact = await getContactByIdResponse.Content.ReadFromJsonAsync<List<ContactDto>>();

            contact.Should().NotBeNull();
            contact[0].PhoneNumber.Should().Be("0799568741");
        }

        private static CreateProfessorDto ProfessorSUT()
        {
            return new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "0799568741"
            };
        }
    }
}
