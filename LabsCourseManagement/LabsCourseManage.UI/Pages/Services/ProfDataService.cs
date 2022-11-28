using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using LabsCourseManagement.Domain;

namespace LabsCourseManage.UI.Pages.Services
{
    public class ProfDataService : IProfDataService
    {
        private const string apiUrl = "http://localhost:7200/v1/api/professors";
        private readonly HttpClient httpClient;

        public ProfDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Professor>> GetAllProfessors()
        {
            return await JsonSerializer
                .DeserializeAsync<IEnumerable<Professor>>
                (await httpClient.GetStreamAsync(apiUrl),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task<Professor> GetProfessorDetail(Guid professorId)
        {
            throw new NotImplementedException();
        }
    }
}
