using LabsCourseManagement.Domain;
using System.Collections.Generic;

namespace LabsCourseManagement.UnitTests
{
    public class ProfessorTests
    {
        [Fact]
        public void When_CreateProfessor_Then_Should_ReturnProfessor()
        {
            //Arrange
            //Act
            var result = Professor.Create("Florin", "Olariu", "0712345678");

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Name.Should().Be("Florin");
            result.Entity.Surname.Should().Be("Olariu");
            result.Entity.ContactInfo.Should().NotBeNull();
            result.Entity.ContactInfo.PhoneNumber.Should().Be("0712345678");
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.Courses.Should().BeEmpty();
            result.Entity.Laboratories.Should().BeEmpty();

        }
        [Fact]
        public void When_CreateProfessorWithNullName_Then_ShouldReturnFailure()
        {
            //Arrange
            //Act
            var result = Professor.Create(null, "surname", "phonenumber");
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Name or surname cannot be null");
        }
        [Fact]
        public void When_CreateProfessorWithNullSurname_Then_ShouldReturnFailure()
        {
            //Arrange
            //Act
            var result = Professor.Create("name", null, "phonenumber");
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be("Name or surname cannot be null");
        }
        [Fact]
        public void When_AddCourses_Then_ShouldAddCourses()
        {
            //Arrange
            var courses = new List<Course>();
            var course1 = Course.Create(".net").Entity;
            var course2 = Course.Create("imr").Entity;
            courses.Add(course1);
            courses.Add(course2);

            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;

            //Act
            var result = professor.AddCourses(courses);

            //Assert
            result.IsSuccess.Should().BeTrue();
            professor.Courses.Should().NotBeNull();
            professor.Courses.Should().HaveCount(2);
            professor.Courses.Should().BeEquivalentTo(courses);

        }
        [Fact]
        public void When_AddLaboratories_Then_ShouldAddLaboratories()
        {
            //Arrange
            var laboratories = new List<Laboratory>();
            var course = Course.Create(".net").Entity;
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            var data = TimeAndPlace.Create(System.DateTime.Parse("1 January 2022"), "C1").Entity;
            var laboratory = Laboratory.Create(".net", course, professor, data).Entity;
            laboratories.Add(laboratory);

            //Act
            var result = professor.AddLaboratories(laboratories);

            //Assert
            result.IsSuccess.Should().BeTrue();
            professor.Laboratories.Should().NotBeNull();
            professor.Laboratories.Should().HaveCount(1);
            professor.Laboratories.Should().BeEquivalentTo(laboratories);

        }
        [Fact]
        public void When_AddNullCourses_Then_ShouldReturnFailure()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            var courses = new List<Course>();

            //Act
            var result = professor.AddCourses(courses);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Courses can not be null");
            professor.Courses.Should().BeEmpty();
        }
        [Fact]
        public void When_AddNullLaboratories_Then_ShouldReturnFailure()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            var laboratories = new List<Laboratory>();

            //Act
            var result = professor.AddLaboratories(laboratories);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Laboratories can not be null");
            professor.Laboratories.Should().BeEmpty();
        }

        [Fact]
        public void When_UpdatePhoneNumber_Then_ShouldUpdatePhoneNumber()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            string newPhoneNumber = "0799556147";

            //Act
            var result = professor.UpdatePhoneNumber(newPhoneNumber);

            //Assert
            result.IsSuccess.Should().BeTrue();
            professor.ContactInfo.PhoneNumber.Should().Be(newPhoneNumber);
        }

        [Fact]
        public void When_UpdatePhoneNumberWithInvalidFormat_Then_ShouldReturnFailure()
        {
            //Arrange
            var professor = Professor.Create("Florin", "Olariu", "0712345678").Entity;
            string newPhoneNumber = "PhoneNumber";

            //Act
            var result = professor.UpdatePhoneNumber(newPhoneNumber);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid phone number format");
        }

    }
}
