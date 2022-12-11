namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class AnnouncementsControllerTests : BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/announcements";

        [Fact]
        public async void When_CreateAnnouncement_Then_ShouldReturnAnnouncementInGetRequest()
        {
            CleanDatabases();
            //Arrange
            var announcementDto = SUT();
            var professorDto = ProfessorSUT();
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

        [Fact]
        public async void When_DeleteAnnouncement_Then_ShouldNotReturnAnnouncementInGetRequest()
        {
            CleanDatabases();
            //Arrange
            var announcementDto = SUT();
            var professorDto = ProfessorSUT();
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createAnnouncementResponse = await HttpClientAnnouncements.PostAsJsonAsync($"{ApiUrl}/{professors[0].Id}", announcementDto);
            var getAnnouncementResponse = await HttpClientAnnouncements.GetAsync(ApiUrl);
            var announcements = await getAnnouncementResponse.Content.ReadFromJsonAsync<List<AnnouncementDto>>();
            var deleteAnnouncementResponse = await HttpClientAnnouncements.DeleteAsync($"{ApiUrl}/{announcements[announcements.Count - 1].Id}");
            var getAnnouncementAfterDeleteResponse = await HttpClientAnnouncements.GetAsync($"{ApiUrl}/{announcements[announcements.Count - 1].Id}");

            //Assert
            deleteAnnouncementResponse.EnsureSuccessStatusCode();
            deleteAnnouncementResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getAnnouncementAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_GetByIdAnnouncement_Then_ShouldGetAnnouncement()
        {
            CleanDatabases();
            //Arrange
            var announcementDto = SUT();
            CreateProfessorDto professorDto = ProfessorSUT();
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync("v1/api/professors", professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync("v1/api/professors");
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Act
            var createAnnouncementResponse = await HttpClientAnnouncements.PostAsJsonAsync($"{ApiUrl}/{professors[0].Id}", announcementDto);
            var getAnnouncementResponse = await HttpClientAnnouncements.GetAsync(ApiUrl);
            var announcements = await getAnnouncementResponse.Content.ReadFromJsonAsync<List<AnnouncementDto>>();
            var getAnnouncementByIdResponse = await HttpClientAnnouncements.GetAsync($"{ApiUrl}/{announcements[announcements.Count - 1].Id}");

            //Assert
            getAnnouncementByIdResponse.EnsureSuccessStatusCode();
            getAnnouncementByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var announcement = await getAnnouncementByIdResponse.Content.ReadFromJsonAsync<AnnouncementDto>();

            announcement.Should().NotBeNull();
            announcement.Id.Should().Be(announcements[announcements.Count - 1].Id);
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
