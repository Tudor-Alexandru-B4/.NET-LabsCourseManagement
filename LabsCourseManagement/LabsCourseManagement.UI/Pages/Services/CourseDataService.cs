using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class CourseDataService : ICourseDataService
    {
        private readonly string apiUrl = UrlString.coursesUrl;
        private readonly HttpClient httpClient;

        public CourseDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<CourseModel>?> GetAllCourses()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<CourseModel>>
                (await httpClient.GetStreamAsync(apiUrl),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task CreateCourse(CourseInput course)
        {
            JsonObject obj = new JsonObject();
            obj.Add("name", course.Name?.ToString());
            obj.Add("professorId", course.ProfessorId.ToString());

            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(apiUrl, content);
        }

        public async Task DeleteCourse(Guid courseId)
        {
            await httpClient.DeleteAsync($"{apiUrl}/{courseId}");
        }

        public async Task AddProfessorsToCourse(Guid courseId, List<Guid> professorsIds)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{courseId}/professors", professorsIds);
        }

        public async Task AddStudentsToCourse(Guid courseId, List<Guid> studentsIds)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{courseId}/students", studentsIds);
        }

    }
}
