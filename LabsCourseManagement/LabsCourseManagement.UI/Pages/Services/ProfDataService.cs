using LabsCourseManagement.Shared.Domain;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class ProfDataService : IProfDataService
    {
        private readonly string apiUrl = UrlString.ProfessorUrl;
        private readonly HttpClient httpClient;

        public ProfDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateProfessor(ProfessorCreateModel professor)
        {
            if(professor.Name == null || professor.Surname == null || professor.PhoneNumber == null)
            {
                return;
            }

            JsonObject obj = new JsonObject();
            obj.Add("name", professor.Name.ToString());
            obj.Add("surname", professor.Surname.ToString());
            obj.Add("phoneNumber", professor.PhoneNumber.ToString());
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(apiUrl, content);
        }

        public async Task DeleteProfessor(Guid professorId)
        {
            await httpClient.DeleteAsync($"{apiUrl}/{professorId}");
        }

        public async Task<IEnumerable<ProfessorModel>?> GetAllProfessors()
        {
            return await System.Text.Json.JsonSerializer
                .DeserializeAsync<IEnumerable<ProfessorModel>>
                (await httpClient.GetStreamAsync(apiUrl),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }

        public async Task AddCourse(Guid courseId,Guid professorId)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{professorId}/courses", new List<Guid>
            {
                courseId
            });

        }
        public async Task UpdateProfessorPhoneNumber(Guid professorId, Guid contactId, string phoneNumber)
        {
            var json = JsonConvert.SerializeObject(phoneNumber);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiUrl}/{professorId}/{contactId}/phoneNumber";
            await httpClient.PostAsync(url, data);
        }

        public async Task UpdateName(string name, Guid professorId)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{professorId}/name", name);
        }

        public async Task UpdateSurname(string surname,Guid professorId)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{professorId}/surname", surname);
        }
    }
}
