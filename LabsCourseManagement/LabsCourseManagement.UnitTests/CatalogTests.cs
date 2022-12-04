using LabsCourseManagement.Domain;
using System.Collections.Generic;

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

        [Fact]
        public void When_AddCatalogStudentGrades_Then_ShouldAddCatalogStudentGrades()
        {
            //Arrange
            var catalog = Catalog.Create().Entity;
            var studentGrades = CreateStudentGradesSUT();

            //Act
            var addStudentGradesResult = catalog.AddStudentGradesToCatalog(studentGrades);

            //Assert
            addStudentGradesResult.IsSuccess.Should().BeTrue();
            catalog.StudentGrades.Should().HaveCount(studentGrades.Count);
            catalog.StudentGrades.Should().BeEquivalentTo(studentGrades);
        }

        [Fact]
        public void When_AddNullCatalogStudentGrades_Then_ShouldReturnFailure()
        {
            //Arrange
            var catalog = Catalog.Create().Entity;
            var studentGrades = CreateStudentGradesSUT();
            studentGrades.Add(null);

            //Act
            var addStudentGradesResult = catalog.AddStudentGradesToCatalog(studentGrades);

            //Assert
            addStudentGradesResult.IsFailure.Should().BeTrue();
            addStudentGradesResult.Error.Should().Be("StudentGrades cannot be null");
        }

        private List<StudentGrades> CreateStudentGradesSUT()
        {
            var student1 = Student.Create("John", "Matt", "A1", 2, "001R002", "0773098001").Entity;
            var student2 = Student.Create("Bobbie", "Bob", "A2", 3, "001R003", "0773098002").Entity;
            var student3 = Student.Create("Akanna", "Anna", "B3", 1, "001R001", "0773098003").Entity;

            var studentGrades1 = StudentGrades.Create(student1).Entity;
            var studentGrades2 = StudentGrades.Create(student2).Entity;
            var studentGrades3 = StudentGrades.Create(student3).Entity;

            return new List<StudentGrades> { studentGrades1, studentGrades2, studentGrades3 };
        }
    }
}
