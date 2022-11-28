using LabsCourseManagement.Infrastructure;
using LabsCourseManagement.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace LabsCourseManagement.IntegrationTests
{
    public class BaseIntegrationTests
    {
        protected HttpClient HttpClientCourses { get; private set; }
        protected HttpClient HttpClientProfessor { get; private set; }
        protected HttpClient HttpClientAnnouncements { get; private set; }
        protected HttpClient HttpClientCatalogs { get; private set; }
        protected HttpClient HttpClientStudents { get; private set; }
        protected HttpClient HttpClientLaboratories { get; private set; }

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
            CleanDatabases();
        }

        protected void CleanDatabases()
        {
            var databaseContext = new DatabaseContext();

            databaseContext.Database.EnsureCreated();

            databaseContext.RemoveRange(databaseContext.Announcements);
            databaseContext.RemoveRange(databaseContext.Catalogs);
            databaseContext.RemoveRange(databaseContext.Contacts);
            databaseContext.RemoveRange(databaseContext.Courses);
            databaseContext.RemoveRange(databaseContext.Grades);
            databaseContext.RemoveRange(databaseContext.GradingInfos);
            databaseContext.RemoveRange(databaseContext.Laboratories);
            databaseContext.RemoveRange(databaseContext.MyStrings);
            databaseContext.RemoveRange(databaseContext.Professors);
            databaseContext.RemoveRange(databaseContext.Students);
            databaseContext.RemoveRange(databaseContext.StudentGrades);
            databaseContext.RemoveRange(databaseContext.TimesAndPlaces);

            databaseContext.SaveChanges();
        }
    }
}
