using LabsCourseManagement.Domain;
using System;

namespace LabsCourseManagement.UnitTests
{
    public class GradeTests
    {
        [Fact]
        public void When_CreateGrade_Then_Should_ReturnGrade()
        {
            //Arrange
            var gradeTuple = CreateGradeSUT();

            //Act
            var result = Grade.Create(gradeTuple.Item1, gradeTuple.Item2, gradeTuple.Item3, gradeTuple.Item4);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.GradingDate.Should().Be(gradeTuple.Item1);
            result.Entity.Mark.Should().Be(gradeTuple.Item2);
            result.Entity.GradeType.Should().Be(gradeTuple.Item3);
            result.Entity.Mentions.Should().Be(gradeTuple.Item4);
        }

        [Fact]
        public void When_CreateGradeWithInvalidMarkUpperBound_Then_Should_ReturnFailure()
        {
            //Arrange
            var gradeTuple = CreateGradeSUT();

            //Act
            var result = Grade.Create(gradeTuple.Item1, 12.0, gradeTuple.Item3, gradeTuple.Item4);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("The mark has to be a value greater than 0 and lower or equal to 10");
        }

        [Fact]
        public void When_CreateGradeWithInvalidMarkLowerBound_Then_Should_ReturnFailure()
        {
            //Arrange
            var gradeTuple = CreateGradeSUT();

            //Act
            var result = Grade.Create(gradeTuple.Item1, -1.0, gradeTuple.Item3, gradeTuple.Item4);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("The mark has to be a value greater than 0 and lower or equal to 10");
        }

        private Tuple<DateTime, double, ExaminationType, string> CreateGradeSUT()
        {
            var gradingDate = DateTime.Parse("1 January 2022");
            var examinationType = ExaminationType.Project;
            var mark = 9.5;
            var mentions = "Custom mentions";

            return Tuple.Create(gradingDate, mark, examinationType, mentions);
        }
    }
}
