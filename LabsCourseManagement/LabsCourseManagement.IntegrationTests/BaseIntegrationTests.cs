using LabsCourseManagement.Infrastructure;
using LabsCourseManagement.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net.Http;

namespace ShelterManagement.API.IntegrationTests
{
    public class BaseIntegrationTests
    {
        protected HttpClient HttpClientProfessor { get; private set; }
        protected BaseIntegrationTests()
        {
            var applicationProfessors = new WebApplicationFactory<ProfessorsController>().WithWebHostBuilder(builder => { });
            HttpClientProfessor = applicationProfessors.CreateClient();
            CleanDatabases();
        }

        private void CleanDatabases()
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