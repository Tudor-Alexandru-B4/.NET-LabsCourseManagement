using System.Text.Json;
using LabsCourseManagement.Shared.Domain;

namespace LabsCourseManage.UI.Pages.Services
{
    public class LaboratoryDataService : ILaboratoryDataService
    {
        private const string ApiURL = "https://localhost:7200/v1/api/laboratories";
        private readonly HttpClient httpClient;

        public LaboratoryDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Laboratory>> GetAllLaboratories()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<Laboratory>>
                (await httpClient.GetStreamAsync(ApiURL),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task<Laboratory> GetLaboratoryDetails(Guid laboratoryId)
        {
            throw new NotImplementedException();
        }
    }
}
