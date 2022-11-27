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
            var course = Course.Create("Test Course").Entity;
            var laboratoryProfessor = Professor.Create("Name", "Surname").Entity;
            var timeAndPlace = TimeAndPlace.Create("C408", DateTime.Parse("1 January 2022")).Entity;

            //Act

            var result = Laboratory.Create(LaboratoryName, course, laboratoryProfessor, timeAndPlace);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Course.Should().NotBeNull();
            result.Entity.Name.Should().Be(LaboratoryName);
            result.Entity.LaboratoryCatalog.Should().NotBeNull();
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.LaboratoryProfessor.Should().NotBeNull();
            result.Entity.LaboratoryStudents.Should().BeEmpty();
            result.Entity.LaboratoryTimeAndPlace.Should().NotBeNull();
            result.Entity.LaboratoryAnnouncements.Should().BeEmpty();
            result.Entity.LaboratoryGradingInfo.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateLaboratoryWithNullName_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create("Test Course").Entity;
            var laboratoryProfessor = Professor.Create("Name", "Surname").Entity;
            var timeAndPlace = TimeAndPlace.Create("C408", DateTime.Parse("1 January 2022")).Entity;

            //Act

            var result = Laboratory.Create(null, course, laboratoryProfessor, timeAndPlace);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Name cannot be null");
        }
    }
}
