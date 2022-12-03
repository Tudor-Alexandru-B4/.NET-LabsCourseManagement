using LabsCourseManagement.Domain;

namespace LabsCourseManagement.UnitTests
{
    public class AnnouncementTests
    {
        [Fact]
        public void When_CreateAnnnouncement_Then_Should_ReturnAnnouncement()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            //Act
            var result = Announcement.Create("header", "text", professor);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Header.Should().Be("header");
            result.Entity.Text.Should().Be("text");
            result.Entity.Writer.Should().Be(professor);
        }
        [Fact]
        public void When_CreateAnnouncementWithNullHeader_Then_ShouldReturnFailure()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            //Act
            var result = Announcement.Create(null, "text", professor);
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Header/text/professor cannot be null");
        }
        [Fact]
        public void When_CreateAnnouncementWithNullText_Then_ShouldReturnFailure()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            //Act
            var result = Announcement.Create("header", null, professor);
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Header/text/professor cannot be null");
        }
        [Fact]
        public void When_CreateAnnouncementWithNullProfessor_Then_ShouldReturnFailure()
        {
            //Arrange
            //Act
            var result = Announcement.Create("header", "text", null);
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Header/text/professor cannot be null");
        }
    }
}
