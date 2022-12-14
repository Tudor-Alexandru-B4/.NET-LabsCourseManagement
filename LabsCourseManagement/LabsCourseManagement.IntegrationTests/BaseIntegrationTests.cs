using LabsCourseManagement.Infrastructure;
using LabsCourseManagement.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;

namespace LabsCourseManagement.IntegrationTests
{
    public class BaseIntegrationTests
    {
        private DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite("Data Source = MyTests.db").Options;
        protected DatabaseContext databaseContext;


        protected HttpClient HttpClientCourses { get; private set; }
        protected HttpClient HttpClientProfessor { get; private set; }
        protected HttpClient HttpClientAnnouncements { get; private set; }
        protected HttpClient HttpClientCatalogs { get; private set; }
        protected HttpClient HttpClientStudents { get; private set; }
        protected HttpClient HttpClientLaboratories { get; private set; }
        protected HttpClient HttpClientContacts { get; private set; }
        protected HttpClient HttpClientInformationStrings { get; private set; }

        protected BaseIntegrationTests()
        {
            var applicationCourses = new WebApplicationFactory<CoursesController>().WithWebHostBuilder(builder => { });
            HttpClientCourses = applicationCourses.CreateClient();
            var applicationProfessors = new WebApplicationFactory<ProfessorsController>().WithWebHostBuilder(builder => { });
            HttpClientProfessor = applicationProfessors.CreateClient();
            var applicationAnnouncements = new WebApplicationFactory<AnnouncementsController>().WithWebHostBuilder(builder => { });
            HttpClientAnnouncements = applicationAnnouncements.CreateClient();
            var applicationCatalogs = new WebApplicationFactory<CatalogsController>().WithWebHostBuilder(builder => { });
            HttpClientCatalogs = applicationCatalogs.CreateClient();
            var applicationStudents = new WebApplicationFactory<StudentsController>().WithWebHostBuilder(builder => { });
            HttpClientStudents = applicationStudents.CreateClient();
            var applicationLaboratories = new WebApplicationFactory<LaboratoriesController>().WithWebHostBuilder(builder => { });
            HttpClientLaboratories = applicationLaboratories.CreateClient();
            var applicationContacts = new WebApplicationFactory<ContactsController>().WithWebHostBuilder(builder => { });
            HttpClientContacts = applicationContacts.CreateClient();
            var applicationInformationStrings = new WebApplicationFactory<InformationStringsController>().WithWebHostBuilder(builder => { });
            HttpClientInformationStrings = applicationInformationStrings.CreateClient();

            databaseContext = new DatabaseContext(options);
        }

        protected void CleanDatabases()
        {
            databaseContext.RemoveRange(databaseContext.Announcements.ToList());
            databaseContext.RemoveRange(databaseContext.Catalogs.ToList());
            databaseContext.RemoveRange(databaseContext.Contacts.ToList());
            databaseContext.RemoveRange(databaseContext.Courses.ToList());
            databaseContext.RemoveRange(databaseContext.Grades.ToList());
            databaseContext.RemoveRange(databaseContext.GradingInfos.ToList());
            databaseContext.RemoveRange(databaseContext.Laboratories.ToList());
            databaseContext.RemoveRange(databaseContext.MyStrings.ToList());
            databaseContext.RemoveRange(databaseContext.Professors.ToList());
            databaseContext.RemoveRange(databaseContext.Students.ToList());
            databaseContext.RemoveRange(databaseContext.StudentGrades.ToList());
            databaseContext.RemoveRange(databaseContext.TimesAndPlaces.ToList());

            databaseContext.SaveChanges();
        }
    }
}
