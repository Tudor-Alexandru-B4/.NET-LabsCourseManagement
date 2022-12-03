using LabsCourseManagement.Domain;

namespace LabsCourseManagement.UnitTests
{
    public class GradingInfoTests
    {
        [Fact]
        public void When_CreateGradingInfo_Then_Should_ReturnGradingInfo()
        {
            //Arrange
            var examinationType = ExaminationType.Project;
            var data = TimeAndPlace.Create(System.DateTime.Parse("1 January 2022"), "C1").Entity;

            //Act
            var result = GradingInfo.Create(examinationType,5, 10,false,"project",data);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.ExaminationType.Should().Be(examinationType);
            result.Entity.IsMandatory.Should().BeFalse();
            result.Entity.MinGrade.Should().Be(5);
            result.Entity.MaxGrade.Should().Be(10);
            result.Entity.Description.Should().Be("project");
            result.Entity.TimeAndPlace.Should().NotBeNull();
            result.Entity.TimeAndPlace.Should().Be(data);
        }
        [Fact]
        public void When_CreateGradingInfoWithNullDescription_Then_ShouldReturnFailure()
        {
            //Arrange
            var examinationType = ExaminationType.Project;
            var data = TimeAndPlace.Create(System.DateTime.Parse("1 January 2022"), "C1").Entity;

            //Act
            var result = GradingInfo.Create(examinationType, 5, 10, false, null, data);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Min grade / max grade cannot be under 0 or description cannot be null");
        }
        [Fact]
        public void When_CreateGradingInfoWithInvalidMinGrade_Then_ShouldReturnFailure()
        {
            //Arrange
            var examinationType = ExaminationType.Project;
            var data = TimeAndPlace.Create(System.DateTime.Parse("1 January 2022"), "C1").Entity;

            //Act
            var result = GradingInfo.Create(examinationType, -1, 10, false, "project", data);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Min grade / max grade cannot be under 0 or description cannot be null");
        }
        [Fact]
        public void When_CreateGradingInfoWithInvalidMaxGrade_Then_ShouldReturnFailure()
        {
            //Arrange
            var examinationType = ExaminationType.Project;
            var data = TimeAndPlace.Create(System.DateTime.Parse("1 January 2022"), "C1").Entity;

            //Act
            var result = GradingInfo.Create(examinationType, 5, -1, false, "project", data);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Min grade / max grade cannot be under 0 or description cannot be null");
        }
    }
}
