using LabsCourseManagement.Domain;
using System;
using System.Collections.Generic;

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
            var laboratoryProfessor = Professor.Create("Name", "Surname", "0711445039").Entity;
            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse("1 January 2022"), "C408").Entity;

            //Act
            var result = Laboratory.Create(LaboratoryName, course, laboratoryProfessor, timeAndPlace);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Course.Should().Be(course);
            result.Entity.Name.Should().Be(LaboratoryName);
            result.Entity.LaboratoryCatalog.Should().NotBeNull();
            result.Entity.IsActive.Should().BeTrue();
            result.Entity.LaboratoryProfessor.Should().Be(laboratoryProfessor);
            result.Entity.LaboratoryStudents.Should().BeEmpty();
            result.Entity.LaboratoryTimeAndPlace.Should().Be(timeAndPlace);
            result.Entity.LaboratoryAnnouncements.Should().BeEmpty();
            result.Entity.LaboratoryGradingInfo.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateLaboratoryWithNullName_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create("Test Course").Entity;
            var laboratoryProfessor = Professor.Create("Name", "Surname", "0733448769").Entity;
            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse("1 January 2022"), "C408").Entity;

            //Act
            var result = Laboratory.Create(null, course, laboratoryProfessor, timeAndPlace);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Name cannot be null");
        }

        [Fact]
        public void When_AddStudents_Then_ShouldAddStudents()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var students = CreateStudentsSUT();

            //Act
            var addStudentsResult = laboratory.AddStudents(students);

            //Assert
            addStudentsResult.IsSuccess.Should().BeTrue();
            laboratory.LaboratoryStudents.Should().HaveCount(3);
            laboratory.LaboratoryStudents.Should().BeEquivalentTo(students);
        }

        [Fact]
        public void When_AddNullStudents_Then_ShouldReturnFailure()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var students = CreateStudentsSUT();
            students.Add(null);

            //Act
            var addStudentsResult = laboratory.AddStudents(students);

            //Assert
            addStudentsResult.IsFailure.Should().BeTrue();
            addStudentsResult.Error.Should().Be("Cannot add null students");
        }

        [Fact]
        public void When_AddLaboratoryAnnouncements_Then_ShouldAddLaboratoryAnnouncements()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var announcements = CreateAnnouncementsSUT(laboratory.LaboratoryProfessor);

            //Act
            var addAnnouncementsResult = laboratory.AddLaboratoryAnnouncements(announcements);

            //Assert
            addAnnouncementsResult.IsSuccess.Should().BeTrue();
            laboratory.LaboratoryAnnouncements.Should().HaveCount(3);
            laboratory.LaboratoryAnnouncements.Should().BeEquivalentTo(announcements);
        }

        [Fact]
        public void When_AddNullLaboratoryAnnouncements_Then_ShouldReturnFailure()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var announcements = CreateAnnouncementsSUT(laboratory.LaboratoryProfessor);
            announcements.Add(null);

            //Act
            var addAnnouncementsResult = laboratory.AddLaboratoryAnnouncements(announcements);

            //Assert
            addAnnouncementsResult.IsFailure.Should().BeTrue();
            addAnnouncementsResult.Error.Should().Be("Cannot add null announcements");
        }

        [Fact]
        public void When_AddLaboratoryGradingInfos_Then_ShouldAddLaboratoryGradingInfos()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var gradingInfos = CreateGradingInfosSUT();
            

            //Act
            var addGradingInfosResult = laboratory.AddLaboratoryGradingInfos(gradingInfos);

            //Assert
            addGradingInfosResult.IsSuccess.Should().BeTrue();
            laboratory.LaboratoryGradingInfo.Should().HaveCount(3);
            laboratory.LaboratoryGradingInfo.Should().BeEquivalentTo(gradingInfos);
        }

        [Fact]
        public void When_AddNullLaboratoryGradingInfos_Then_ShouldReturnFailure()
        {
            //Arrange
            var laboratory = CreateLaboratorySUT();
            var gradingInfos = CreateGradingInfosSUT();
            gradingInfos.Add(null);

            //Act
            var addGradingInfosResult = laboratory.AddLaboratoryGradingInfos(gradingInfos);

            //Assert
            addGradingInfosResult.IsFailure.Should().BeTrue();
            addGradingInfosResult.Error.Should().Be("Cannot add null grading infos");
        }

        private Laboratory CreateLaboratorySUT()
        {
            var course = Course.Create("Test Course").Entity;
            var laboratoryProfessor = Professor.Create("Name", "Surname", "0755776138").Entity;
            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse("1 January 2022"), "C408").Entity;
            var laboratory = Laboratory.Create(LaboratoryName, course, laboratoryProfessor, timeAndPlace).Entity;
            return laboratory;
        }

        private List<Student> CreateStudentsSUT()
        {
            var student1 = Student.Create("John", "Matt", "A1", 2, "001R002", "0773098001").Entity;
            var student2 = Student.Create("Bobbie", "Bob", "A2", 3, "001R003", "0773098002").Entity;
            var student3 = Student.Create("Akanna", "Anna", "B3", 1, "001R001", "0773098003").Entity;

            var students = new List<Student> { student1, student2, student3 };
            return students;
        }

        private List<Announcement> CreateAnnouncementsSUT(Professor laboratoryProfessor)
        {
            var announcement1 = Announcement.Create("Project Deadline", "Project deadline is tomorrow!", laboratoryProfessor).Entity;
            var announcement2 = Announcement.Create("Test date", "Don't forget that you will take the tests today!", laboratoryProfessor).Entity;
            var announcement3 = Announcement.Create("Holiday news", "Merry Christmas!", laboratoryProfessor).Entity;

            var announcements = new List<Announcement> { announcement1, announcement2, announcement3 };
            return announcements;
        }

        private List<GradingInfo> CreateGradingInfosSUT()
        {
            var gradingInfo1 = GradingInfo.Create(ExaminationType.Test, 5, 10, true, "This is a mandatory test!", TimeAndPlace.Create(DateTime.Now, "C2").Entity).Entity;
            var gradingInfo2 = GradingInfo.Create(ExaminationType.Project, 7, 10, false, "Choose your project from this list: link", TimeAndPlace.Create(DateTime.Now, "C3").Entity).Entity;
            var gradingInfo3 = GradingInfo.Create(ExaminationType.MidTermExam, 2.5, 10, true, "The midterm examination will be on a Saturday.", TimeAndPlace.Create(DateTime.Now, "C112").Entity).Entity;

            var gradingInfos = new List<GradingInfo> { gradingInfo1, gradingInfo2, gradingInfo3 };
            return gradingInfos;
        }
    }
}
