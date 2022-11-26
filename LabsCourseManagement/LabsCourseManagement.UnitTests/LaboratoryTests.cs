using LabsCourseManagement.Domain;
using System;

namespace LabsCourseManagement.UnitTests
{
    public class LaboratoryTests
    {
        private const string LaboratoryName = "TestLaboratory";

        [Fact]
        public void When_CreateLaboratory_Then_ShouldReturnLaboratory()
        {
            //Arrange
            var course = new Course();
            var laboratoryProfessor = new Professor();
            var timeAndPlace = new TimeAndPlace();

            //Act

            //Create(string name, Course course, 
            //Professor laboratoryProfessor, timeandplace)

            var result = Laboratory.Create(LaboratoryName, course.Id, laboratoryProfessor, timeandplace);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Course.Should().NotBeNull();
            result.Entity.Name.Should().Be(LaboratoryName);
            result.Entity.LaboratoryCatalog.Should().NotBeNull();
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.LaboratoryProfessor.Should().BeNull();
            result.Entity.LaboratoryStudents.Should().BeEmpty();
            result.Entity.LaboratoryTimeAndPlace.Should().BeNull();
            result.Entity.LaboratoryAnnouncements.Should().BeEmpty();
            result.Entity.LaboratoryGradingInfo.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateLaboratoryWithNullName_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = new Course();

            //Act
            var result = Laboratory.Create(null, course.Id);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Name cannot be null");
        }
    }
}
