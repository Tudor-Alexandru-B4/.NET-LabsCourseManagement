using LabsCourseManagement.Domain;
using System;
using System.Collections.Generic;

namespace LabsCourseManagement.UnitTests
{
    public class StudentTests
    {
        private const string studentName = "Name";
        private const string studentSurname = "Surname";
        private const string group = "Group";
        private const int year = 3;
        private const string registrationNumber = "Nr matricol";
        private const string phoneNumber = "1234567890";

        [Fact]
        public void When_CreateStudent_Then_ShouldReturnStudent()
        {
            // Arrange
            // Act
            var result = Student.Create(studentName, studentSurname, group, year, registrationNumber, phoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.StudentId.Should().NotBeEmpty();
            result.Entity.Name.Should().Be(studentName);
            result.Entity.Surname.Should().Be(studentSurname);
            result.Entity.ContactInfo.Should().NotBeNull();
            result.Entity.Year.Should().Be(year);
            result.Entity.Group.Should().Be(group);
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.RegistrationNumber.Should().Be(registrationNumber);
            result.Entity.Courses.Should().BeEmpty();
            result.Entity.Laboratories.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateStudentWithNullField_Then_ShouldReturnFailure()
        {
            // Arrange
            // Act
            var result = Student.Create(null, studentSurname, group, year, registrationNumber, phoneNumber);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("The field cannot be null / year cannot be less than 1");
        }

        [Fact]
        public void When_AddCourses_Then_ShouldAddCourses()
        {
            // Arrange
            var courses = CreateCoursesSUT();
            var student = CreateStudentSUT();

            // Act
            var addCoursesResult = student.AddCourse(courses);

            // Assert
            addCoursesResult.IsSuccess.Should().BeTrue();
            student.Courses.Should().HaveCount(3);
            student.Courses.Should().BeEquivalentTo(courses);
        }

        [Fact]
        public void When_AddLaboratories_Then_ShouldAddLaboratories()
        {
            // Arrange
            var laboratories = ReturnLaboratoriesSUT();
            var student = CreateStudentSUT();

            // Act
            var addLaboratoriesResult = student.AddLaboratories(laboratories);

            // Assert
            addLaboratoriesResult.IsSuccess.Should().BeTrue();
            student.Laboratories.Should().NotBeEmpty();
            student.Laboratories.Should().HaveCount(3);
            student.Laboratories.Should().BeEquivalentTo(laboratories);
        }

        [Fact]
        public void When_AddNullCourses_Then_ShouldReturnFailure()
        {
            // Arrange
            var student = CreateStudentSUT();
            var courses = CreateCoursesSUT();
            courses.Add(null);

            // Act
            var addCoursesResult = student.AddCourse(courses);

            // Assert
            addCoursesResult.IsFailure.Should().BeTrue();
            addCoursesResult.Error.Should().Be("Courses cannot be null");
            student.Courses.Should().BeEmpty();
        }

        [Fact]
        public void When_AddNullLaboratories_Then_ShouldReturnFailure()
        {
            // Arrange
            var student = CreateStudentSUT();
            var laboratories = ReturnLaboratoriesSUT();
            laboratories.Add(null);

            // Act
            var addLaboratoriesResult = student.AddLaboratories(laboratories);

            // Assert
            addLaboratoriesResult.IsFailure.Should().BeTrue();
            addLaboratoriesResult.Error.Should().Be("Laboratories cannot be null");
            student.Laboratories.Should().BeEmpty();
        }

        [Fact]
        public void When_UpdateName_Then_ShouldUpdateName()
        {
            //Arrange
            var student = CreateStudentSUT();
            string name = "Ioan";

            //Act
            var result = student.UpdateName(name);

            //Assert
            result.IsSuccess.Should().BeTrue();
            student.Name.Should().Be(name);
        }
        [Fact]
        public void When_UpdateNameWithNull_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = CreateStudentSUT();
            string name = null;

            //Act
            var result = student.UpdateName(name);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Name cannot be null");
        }
        [Fact]
        public void When_UpdateSurname_Then_ShouldUpdateSurname()
        {
            //Arrange
            var student = CreateStudentSUT();
            string surname = "Popescu";

            //Act
            var result = student.UpdateSurname(surname);

            //Assert
            result.IsSuccess.Should().BeTrue();
            student.Surname.Should().Be(surname);
        }
        [Fact]
        public void When_UpdateSurnameWithNull_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = CreateStudentSUT();
            string surname = null;

            //Act
            var result = student.UpdateSurname(surname);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Surname cannot be null");
        }
        [Fact]
        public void When_UpdateGroup_Then_ShouldChangeGroup()
        {
            //Arrange
            var student = CreateStudentSUT();
            string newGroup = "C1";

            //Act
            var result = student.UpdateGroup(newGroup);

            //Assert
            result.IsSuccess.Should().BeTrue();
            student.Group.Should().Be(newGroup);
        }

        [Fact]
        public void When_UpdateGroupWithNull_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = CreateStudentSUT();
            string group = null;

            //Act
            var result = student.UpdateGroup(group);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Group cannot be null");

        }

        [Fact]
        public void When_UpdateYear_Then_ShouldUpdateYear()
        {
            //Arrange
            var student = CreateStudentSUT();
            int year = 6;

            //Act
            var result = student.UpdateYear(year);

            //Assert
            result.IsSuccess.Should().BeTrue();
            student.Year.Should().Be(year);
        }
        [Fact]
        public void When_UpdateYearWithValueLessThen1_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = CreateStudentSUT();
            int year = 0;

            //Act
            var result = student.UpdateYear(year);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Year cannot be less then 1");
        }
        [Fact]
        public void When_UpdateRegistrationNumber_Then_ShouldUpdateRegistrationNumber()
        {
            //Arrange
            var student = CreateStudentSUT();
            string registrationNumber = "310910401RSL7217392";

            //Act
            var result = student.UpdateRegistrationNumber(registrationNumber);

            //Assert
            result.IsSuccess.Should().BeTrue();
            student.RegistrationNumber.Should().Be(registrationNumber);
        }

        [Fact]
        public void When_UpdateRegistrationNumberWithNull_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = CreateStudentSUT();
            string registrationNumber = null;

            //Act
            var result = student.UpdateRegistrationNumber(registrationNumber);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Registration number cannot be null");
        }


        private List<Course> CreateCoursesSUT()
        {
            var course1 = Course.Create("Course1").Entity;
            var course2 = Course.Create("Course2").Entity;
            var course3 = Course.Create("Course3").Entity;

            var courses = new List<Course> { course1, course2, course3 };
            return courses;
        }

        private List<Laboratory> ReturnLaboratoriesSUT()
        {
            var laboratory1 = CreateLaboratoriesSut("Laboratory1", "Course1", "Name1", "Surname1", "C1", "1 January 2022");
            var laboratory2 = CreateLaboratoriesSut("Laboratory2", "Course2", "Name2", "Surname2", "C2", "2 January 2022");
            var laboratory3 = CreateLaboratoriesSut("Laboratory3", "Course3", "Name3", "Surname3", "C3", "3 January 2022");

            var laboratories = new List<Laboratory> { laboratory1, laboratory2, laboratory3 };
            return laboratories;
        }

        private Laboratory CreateLaboratoriesSut(string laboratoryName, string courseName, string professorName,
            string professorSurname, string classroom, string dateTime)
        {
            var course = Course.Create(courseName).Entity;
            var laboratoryProfessor = Professor.Create(professorName, professorSurname, "0755116783").Entity;
            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse(dateTime), classroom).Entity;
            var laboratory = Laboratory.Create(laboratoryName, course, laboratoryProfessor, timeAndPlace).Entity;
            return laboratory;
        }

        private Student CreateStudentSUT()
        {
            var student = Student.Create(studentName, studentSurname, group, year, registrationNumber, phoneNumber).Entity;
            return student;
        }
    }
}
