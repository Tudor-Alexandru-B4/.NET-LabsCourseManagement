using LabsCourseManagement.Shared.Domain;
using System.Text.Json;


namespace LabsCourseManagement.UI.Pages.Services
{
    public class ProfDataService : IProfDataService
    {
        private const string apiUrl = "https://localhost:7200/v1/api/professors";
        private readonly HttpClient httpClient;

        public ProfDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<ProfessorModel>> GetAllProfessors()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<ProfessorModel>>
                (await httpClient.GetStreamAsync(apiUrl),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task<ProfessorModel> GetProfessorDetail(Guid professorId)
        {
            throw new NotImplementedException();
        }
    }
}
