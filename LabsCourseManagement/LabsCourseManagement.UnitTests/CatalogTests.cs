using LabsCourseManagement.Domain;

namespace LabsCourseManagement.UnitTests
{
    public class CatalogTests
    {
        [Fact]
        public void When_CreateCatalog_Then_Should_ReturnCatalog()
        {
            //Arrange

            //Act
            var result = Catalog.Create();

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.StudentGrades.Should().BeEmpty();
        }
    }
}
