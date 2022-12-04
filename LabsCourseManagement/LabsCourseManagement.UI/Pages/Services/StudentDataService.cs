using LabsCourseManagement.Shared.Domain;
using System.Text.Json;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class StudentDataService : IStudentDataService
    {
        private const string ApiURL = "https://localhost:7230/v1/api/students";
        private readonly HttpClient httpClient;

        public StudentDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<StudentModel>> GetAllStudent()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<StudentModel>>
                (await httpClient.GetStreamAsync(ApiURL),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task<StudentModel> GetStudentDetail(Guid studentId)
        {
            throw new NotImplementedException();
        }
    }
}
