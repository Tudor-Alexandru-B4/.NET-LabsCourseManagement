using LabsCourseManagement.WebUI.Dtos;

namespace LabsCourseManagement.IntegrationTests
{
    public class AnnouncementsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/announcements";

        [Fact]
        public async void When_CreateAnnouncement_Then_ShouldReturnAnnouncementInGetRequest()
        {
            //Arrange
            var announcementDto = SUT();
            var professorDto = new CreateProfessorDto()
            {
                Name = "ProfessorName",
                Surname = "ProfessorSurname",
                PhoneNumber = "ProfessorPhoneNumber"
            };
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createAnnouncementResponse = await HttpClientAnnouncements.PostAsJsonAsync($"{ApiUrl}/{professors[0].Id}", announcementDto);
            var getAnnouncementResponse = await HttpClientAnnouncements.GetAsync(ApiUrl);

            //Assert
            createAnnouncementResponse.EnsureSuccessStatusCode();
            createAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getAnnouncementResponse.EnsureSuccessStatusCode();
            var announcements = await getAnnouncementResponse.Content.ReadFromJsonAsync<List<AnnouncementDto>>();

            announcements.Should().NotBeNull();
            announcements.Should().NotBeEmpty();
            announcements.Count.Should().Be(1);
        }

        private static CreateAnnouncementDto SUT()
        {
            return new CreateAnnouncementDto()
            {
                Header = "Header",
                Text = "Text"
            };
        }
    }
}
