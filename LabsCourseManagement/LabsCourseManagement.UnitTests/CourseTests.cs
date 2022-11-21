using LabsCourseManagement.Domain;
using System.Collections.Generic;

namespace LabsCourseManagement.UnitTests
{
    public class CourseTests
    {
        private const string CourseName = "TestCourse";

        [Fact]
        public void When_CreateCourse_Then_ShouldReturnCourse()
        {
            //Arrange

            //Act
            var result = Course.Create(CourseName);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Name.Should().Be(CourseName);
            result.Entity.CourseCatalog.Should().NotBeNull();
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.Professors.Should().BeEmpty();
            result.Entity.Laboratorys.Should().BeEmpty();
            result.Entity.CourseStudents.Should().BeEmpty();
            result.Entity.CourseProgram.Should().BeEmpty();
            result.Entity.CourseAnnouncements.Should().BeEmpty();
            result.Entity.CourseGradingInfo.Should().BeEmpty();
            result.Entity.HelpfulMaterials.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateCourseWithNullName_Then_ShouldReturnFailure()
        {
            //Arrange

            //Act
            var result = Course.Create(null);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Name cannot be null");
        }

        [Fact]
        public void When_AddHelpfulMaterials_Then_ShouldAddHelpfulMaterialsToList()
        {
            //Arrange
            List<MyString> helpfulMeterials = new List<MyString>()
            {
                MyString.Create("HelpfulMaterial-1").Entity,
                MyString.Create("HelpfulMaterial-2").Entity,
                MyString.Create("HelpfulMaterial-3").Entity
            };

            //Act
            var result = Course.Create(CourseName);
            var addResult = result.Entity.AddHelpfulMaterials(helpfulMeterials);

            //Assert
            addResult.IsSuccess.Should().BeTrue();
            result.Entity.HelpfulMaterials.Should().NotBeEmpty();
            result.Entity.HelpfulMaterials.Count.Should().Be(3);
            result.Entity.HelpfulMaterials.Should().Equal(helpfulMeterials);
        }

        [Fact]
        public void When_AddHelpfulMaterialsEmpty_Then_ShouldReturnFailure()
        {
            //Arrange
            List<MyString> helpfulMeterials = new List<MyString>();

            //Act
            var result = Course.Create(CourseName);
            var addResult = result.Entity.AddHelpfulMaterials(helpfulMeterials);

            //Assert
            addResult.IsFailure.Should().BeTrue();
            addResult.Error.Should().Be("HelpfulMaterials cannot be null");
            result.Entity.HelpfulMaterials.Should().BeEmpty();
        }
    }
}
