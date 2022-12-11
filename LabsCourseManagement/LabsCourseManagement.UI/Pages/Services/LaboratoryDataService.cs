using LabsCourseManagement.Shared.Domain;
using System.Text.Json;


namespace LabsCourseManagement.UI.Pages.Services
{
    public class LaboratoryDataService : ILaboratoryDataService
    {
        private readonly string apiUrl = new Uri("https://localhost:7200/v1/api/laboratories").ToString();
        private readonly HttpClient httpClient;

        public LaboratoryDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<LaboratoryModel>?> GetAllLaboratories()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<LaboratoryModel>>
                (await httpClient.GetStreamAsync(apiUrl),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }
    }
}
