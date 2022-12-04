using LabsCourseManagement.Shared.Domain;
using System.Text.Json;


namespace LabsCourseManagement.UI.Pages.Services
{
    public class LaboratoryDataService : ILaboratoryDataService
    {
        private const string ApiURL = "https://localhost:7200/v1/api/laboratories";
        private readonly HttpClient httpClient;

        public LaboratoryDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<LaboratoryModel>> GetAllLaboratories()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<LaboratoryModel>>
                (await httpClient.GetStreamAsync(ApiURL),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task<LaboratoryModel> GetLaboratoryDetails(Guid laboratoryId)
        {
            throw new NotImplementedException();
        }
    }
}
