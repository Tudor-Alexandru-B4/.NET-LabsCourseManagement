using LabsCourseManagement.Domain;
using Microsoft.VisualBasic;
using System;

namespace LabsCourseManagement.UnitTests
{
    public class TimeAndPlaceTests
    {
        private DateTime dateTime = DateTime.Now;
        private const string classroom = "C409";

        [Fact]
        public void When_CreateTimeAndPlace_Then_ShouldReturnTimeAndPlace()
        {
            // Arrange
            // Act
            var result = TimeAndPlace.Create(dateTime, classroom);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.DateAndTime.Should().Be(dateTime);    
            result.Entity.Classroom.Should().Be(classroom);
        }

        [Fact]
        public void When_CreateTimeAndPlacetWithNullClassroom_Then_ShouldReturnFailure()
        {
            // Arrange
            // Act
            var result = TimeAndPlace.Create(dateTime, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Classroom cannot be null");
        }
    }
}
