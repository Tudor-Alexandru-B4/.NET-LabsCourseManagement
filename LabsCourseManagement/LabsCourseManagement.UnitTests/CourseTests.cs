using LabsCourseManagement.Domain;
using System;
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
            result.Entity.Laboratories.Should().BeEmpty();
            result.Entity.Students.Should().BeEmpty();
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
            List<InformationString> helpfulMeterials = new List<InformationString>()
            {
                InformationString.Create("HelpfulMaterial-1").Entity,
                InformationString.Create("HelpfulMaterial-2").Entity,
                InformationString.Create("HelpfulMaterial-3").Entity
            };

            //Act
            var result = Course.Create(CourseName);
            var addResult = result.Entity.AddHelpfulMaterials(helpfulMeterials);

            //Assert
            addResult.IsSuccess.Should().BeTrue();
            result.Entity.HelpfulMaterials.Should().NotBeEmpty();
            result.Entity.HelpfulMaterials.Count.Should().Be(helpfulMeterials.Count);
            result.Entity.HelpfulMaterials.Should().Equal(helpfulMeterials);
        }

        [Fact]
        public void When_AddHelpfulMaterialsEmpty_Then_ShouldReturnFailure()
        {
            //Arrange
            List<InformationString> helpfulMeterials = new List<InformationString>();
            helpfulMeterials.Add(null);

            //Act
            var result = Course.Create(CourseName);
            var addResult = result.Entity.AddHelpfulMaterials(helpfulMeterials);

            //Assert
            addResult.IsFailure.Should().BeTrue();
            addResult.Error.Should().Be("HelpfulMaterials cannot be null");
            result.Entity.HelpfulMaterials.Should().BeEmpty();
        }

        [Fact]
        public void When_AddCourseAnnouncements_Then_ShouldAddCourseAnnouncements()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var announcements = CreateAnnouncementsSUT();

            //Act
            var addAnnouncementsResult = course.AddCourseAnnouncements(announcements);

            //Assert
            addAnnouncementsResult.IsSuccess.Should().BeTrue();
            course.CourseAnnouncements.Should().HaveCount(announcements.Count);
            course.CourseAnnouncements.Should().BeEquivalentTo(announcements);
        }

        [Fact]
        public void When_AddNullCourseAnnouncements_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var announcements = CreateAnnouncementsSUT();
            announcements.Add(null);

            //Act
            var addAnnouncementsResult = course.AddCourseAnnouncements(announcements);

            //Assert
            addAnnouncementsResult.IsFailure.Should().BeTrue();
            addAnnouncementsResult.Error.Should().Be("Announcements cannot be null");
        }

        [Fact]
        public void When_AddCourseProfessors_Then_ShouldAddCourseProfessors()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var professors = CreateProfessorsSUT();

            //Act
            var addProfessorsResult = course.AddProfessors(professors);

            //Assert
            addProfessorsResult.IsSuccess.Should().BeTrue();
            course.Professors.Should().HaveCount(professors.Count);
            course.Professors.Should().BeEquivalentTo(professors);
        }

        [Fact]
        public void When_AddNullCourseProfessors_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var professors = CreateProfessorsSUT();
            professors.Add(null);

            //Act
            var addProfessorsResult = course.AddProfessors(professors);

            //Assert
            addProfessorsResult.IsFailure.Should().BeTrue();
            addProfessorsResult.Error.Should().Be("Professors cannot be null");
        }

        [Fact]
        public void When_AddCourseLaboratories_Then_ShouldAddCourseLaboratories()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var laboratories = CreateLaboratoriesSUT();

            //Act
            var addLaboratoriesResult = course.AddLaboratories(laboratories);

            //Assert
            addLaboratoriesResult.IsSuccess.Should().BeTrue();
            course.Laboratories.Should().HaveCount(laboratories.Count);
            course.Laboratories.Should().BeEquivalentTo(laboratories);
        }

        [Fact]
        public void When_AddNullCourseLaboratories_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var laboratories = CreateLaboratoriesSUT();
            laboratories.Add(null);

            //Act
            var addLaboratoriesResult = course.AddLaboratories(laboratories);

            //Assert
            addLaboratoriesResult.IsFailure.Should().BeTrue();
            addLaboratoriesResult.Error.Should().Be("Laboratories cannot be null");
        }

        [Fact]
        public void When_AddCourseStudents_Then_ShouldAddCourseStudents()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var students = CreateStudentsSUT();

            //Act
            var addStudentsResult = course.AddCourseStudents(students);

            //Assert
            addStudentsResult.IsSuccess.Should().BeTrue();
            course.Students.Should().HaveCount(students.Count);
            course.Students.Should().BeEquivalentTo(students);
        }

        [Fact]
        public void When_AddNullStudentsAnnouncements_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var students = CreateStudentsSUT();
            students.Add(null);

            //Act
            var addStudentsResult = course.AddCourseStudents(students);

            //Assert
            addStudentsResult.IsFailure.Should().BeTrue();
            addStudentsResult.Error.Should().Be("Students cannot be null");
        }

        [Fact]
        public void When_AddCourseTimeAndPlaces_Then_ShouldAddCourseTimeAndPlaces()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var timeAndPlaces = CreateTimeAndPlacesSUT();

            //Act
            var addTimeAndPlacesProfessorsResult = course.AddCoursePrograms(timeAndPlaces);

            //Assert
            addTimeAndPlacesProfessorsResult.IsSuccess.Should().BeTrue();
            course.CourseProgram.Should().HaveCount(timeAndPlaces.Count);
            course.CourseProgram.Should().BeEquivalentTo(timeAndPlaces);
        }

        [Fact]
        public void When_AddNullProfessorsTimeAndPlaces_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var timeAndPlaces = CreateTimeAndPlacesSUT();
            timeAndPlaces.Add(null);

            //Act
            var addTimeAndPlacesProfessorsResult = course.AddCoursePrograms(timeAndPlaces);

            //Assert
            addTimeAndPlacesProfessorsResult.IsFailure.Should().BeTrue();
            addTimeAndPlacesProfessorsResult.Error.Should().Be("TimesAndPlaces cannot be null");
        }

        [Fact]
        public void When_AddCourseGradingInfos_Then_ShouldAddCourseGradingInfos()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var gradingInfos = CreateGradingInfosSUT();

            //Act
            var addGradingInfosResult = course.AddCourseGradingInfos(gradingInfos);

            //Assert
            addGradingInfosResult.IsSuccess.Should().BeTrue();
            course.CourseGradingInfo.Should().HaveCount(gradingInfos.Count);
            course.CourseGradingInfo.Should().BeEquivalentTo(gradingInfos);
        }

        [Fact]
        public void When_AddNullCourseGradingInfos_Then_ShouldReturnFailure()
        {
            //Arrange
            var course = Course.Create(CourseName).Entity;
            var gradingInfos = CreateGradingInfosSUT();
            gradingInfos.Add(null);

            //Act
            var addGradingInfosResult = course.AddCourseGradingInfos(gradingInfos);

            //Assert
            addGradingInfosResult.IsFailure.Should().BeTrue();
            addGradingInfosResult.Error.Should().Be("GradingInfos cannot be null");
        }

        private List<Announcement> CreateAnnouncementsSUT()
        {
            var professor = Professor.Create("John", "Jhonny", "0700000000").Entity;
            var announcement1 = Announcement.Create("Project Deadline", "Project deadline is tomorrow!", professor).Entity;
            var announcement2 = Announcement.Create("Test date", "Don't forget that you will take the tests today!", professor).Entity;
            var announcement3 = Announcement.Create("Holiday news", "Merry Christmas!", professor).Entity;

            var announcements = new List<Announcement> { announcement1, announcement2, announcement3 };
            return announcements;
        }

        private List<Professor> CreateProfessorsSUT()
        {
            var professor1 = Professor.Create("Doinaru", "Mihaela", "0766485137").Entity;
            var professor2 = Professor.Create("Popescu", "Mihai", "0767514337").Entity;
            var professor3 = Professor.Create("Basescu", "Decebal", "0744377666").Entity;

            var professors = new List<Professor> { professor1, professor2, professor3 };
            return professors;
        }

        private List<Laboratory> CreateLaboratoriesSUT()
        {
            var professor = Professor.Create("Toader", "Iulian", "0721346537").Entity;
            var course = Course.Create(".BET").Entity;

            var laboratory1 = Laboratory.Create("+BET", course, professor, TimeAndPlace.Create(System.DateTime.Parse("14:58"), "C416").Entity).Entity;
            var laboratory2 = Laboratory.Create("+BET", course, professor, TimeAndPlace.Create(System.DateTime.Parse("13:22"), "C201").Entity).Entity;
            var laboratory3 = Laboratory.Create("+BET", course, professor, TimeAndPlace.Create(System.DateTime.Parse("20:21"), "C109").Entity).Entity;

            return new List<Laboratory> { laboratory1, laboratory2, laboratory3 };
        }

        private List<Student> CreateStudentsSUT()
        {
            var student1 = Student.Create("John", "Matt", "A1", 2, "001R002", "0773098001").Entity;
            var student2 = Student.Create("Bobbie", "Bob", "A2", 3, "001R003", "0773098002").Entity;
            var student3 = Student.Create("Akanna", "Anna", "B3", 1, "001R001", "0773098003").Entity;

            var students = new List<Student> { student1, student2, student3 };
            return students;
        }

        private List<TimeAndPlace> CreateTimeAndPlacesSUT()
        {
            var timeAndPlace1 = TimeAndPlace.Create(System.DateTime.Parse("14:58"), "C416").Entity;
            var timeAndPlace2 = TimeAndPlace.Create(System.DateTime.Parse("13:22"), "C201").Entity;
            var timeAndPlace3 = TimeAndPlace.Create(System.DateTime.Parse("20:21"), "C109").Entity;
        
            return new List<TimeAndPlace> { timeAndPlace1, timeAndPlace2, timeAndPlace3 };
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
