using LabsCourseManagement.Domain;
using System;
using System.Diagnostics;

namespace LabsCourseManagement.UnitTests
{
    public class StudentGradesTests
    {
        [Fact]
        public void When_CreateStudentGrades_Then_Should_ReturnStudentGrades()
        {
            //Arrange
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;

            //Act
            var result = StudentGrades.Create(student);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Student.Should().Be(student);
            result.Entity.Grades.Should().BeEmpty();
            result.Entity.FinalGrade.Should().BeNull();
        }

        [Fact]
        public void When_CreateStudentGradesForNullStudent_Then_Should_ReturnFailure()
        {
            //Arrange

            //Act
            var result = StudentGrades.Create(null);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("The student cannot be null");
        }

        [Fact]
        public void When_AddGrade_Then_Should_AddGrade()
        {
            //Arrange
            var gradingDate = DateTime.Parse("1 January 2022");
            var mark = 9.5;
            var examinationType = ExaminationType.Project;
            var mentions = "Custom mentions";
            var grade = Grade.Create(gradingDate, mark, examinationType, mentions).Entity;
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var result = studentGrades.AddGrade(grade);

            //Assert
            result.IsSuccess.Should().BeTrue();
            studentGrades.Grades.Should().NotBeEmpty();
            studentGrades.Grades.Count.Should().Be(1);
            studentGrades.Grades[0].Should().Be(grade);
        }

        [Fact]
        public void When_AddNullGrade_Then_Should_ReturnFailure()
        {
            //Arrange
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var result = studentGrades.AddGrade(null);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("The grade cannot be null");
        }

        [Fact]
        public void When_RemoveGrade_Then_Should_RemoveGrade()
        {
            //Arrange
            var gradingDate = DateTime.Parse("1 January 2022");
            var mark = 9.5;
            var examinationType = ExaminationType.Project;
            var mentions = "Custom mentions";
            var grade = Grade.Create(gradingDate, mark, examinationType, mentions).Entity;
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var result = studentGrades.AddGrade(grade);
            var removeResult = studentGrades.RemoveGrade(grade);

            //Assert
            removeResult.IsSuccess.Should().BeTrue();
            studentGrades.Grades.Count.Should().Be(0);
        }

        [Fact]
        public void When_RemoveNullGrade_Then_Should_ReturnFailure()
        {
            //Arrange
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var removeResult = studentGrades.RemoveGrade(null);

            //Assert
            removeResult.IsFailure.Should().BeTrue();
            removeResult.Error.Should().Be("The grade cannot be null");
        }

        [Fact]
        public void When_ComputeFinalMark_Then_ShouldSetFinalMark()
        {
            //Arrange
            var gradingDate = DateTime.Parse("1 January 2022");
            var mark = 9.5;
            var examinationType = ExaminationType.Project;
            var mentions = "Custom mentions";
            var grade = Grade.Create(gradingDate, mark, examinationType, mentions).Entity;
            var grade1 = Grade.Create(gradingDate, mark, examinationType, mentions).Entity;
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var result = studentGrades.AddGrade(grade);
            result = studentGrades.AddGrade(grade1);
            var computeResult = studentGrades.ComputeFinalGrade();

            //Assert
            computeResult.IsSuccess.Should().BeTrue();
            studentGrades.FinalGrade.Mark.Should().Be(mark);
        }

        [Fact]
        public void When_ComputeFinalMarkWithoutGrades_Then_ShouldReturnFailure()
        {
            //Arrange
            var student = Student.Create("Name", "Surname", "A1", 2, "001R001", "0773098000").Entity;
            var studentGrades = StudentGrades.Create(student).Entity;

            //Act
            var computeresult = studentGrades.ComputeFinalGrade();

            //Assert
            computeresult.IsFailure.Should().BeTrue();
            computeresult.Error.Should().Be("Cannot compute final grade without any grades");
        }
    }
}
