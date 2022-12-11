using LabsCourseManagement.Shared.Domain;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class ProfDataService : IProfDataService
    {
        private string apiUrl = new Uri("https://localhost:7200/v1/api/professors").ToString();
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
            await httpClient.DeleteAsync(new Uri(apiUrl + "/" + professorId.ToString()).ToString());
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
            var url = new Uri(apiUrl + "/" + professorId.ToString() + "/" + contactId.ToString() + "/" + "phoneNumber").ToString();
            await httpClient.PostAsync(url, data);
        }
    }
}
