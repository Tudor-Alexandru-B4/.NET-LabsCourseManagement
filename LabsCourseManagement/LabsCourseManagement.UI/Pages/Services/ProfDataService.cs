using LabsCourseManagement.Shared.Domain;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;
using System;
using Newtonsoft.Json;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class ProfDataService : IProfDataService
    {
        private const string apiUrl = "https://localhost:7200/v1/api/Professors";
        private readonly HttpClient httpClient;

        public ProfDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateProfessor(ProfessorCreateModel professor)
        {
            JsonObject obj = new JsonObject();
            obj.Add("name", professor.Name.ToString());
            obj.Add("surname", professor.Surname.ToString());
            obj.Add("phoneNumber", professor.PhoneNumber.ToString());
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(apiUrl, content);
        }

        public async Task DeleteProfessor(Guid professorId)
        {
            await httpClient.DeleteAsync(apiUrl + "/" + professorId.ToString());
        }

        public async Task<IEnumerable<ProfessorModel>> GetAllProfessors()
        {
            return await System.Text.Json.JsonSerializer
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

        public async Task UpdateProfessorPhoneNumber(Guid professorId, Guid contactId, string phoneNumber)
        {
            //JsonObject obj = new JsonObject();
            //obj.Add("phoneNumber", phoneNumber.ToString());
            //var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            var json = JsonConvert.SerializeObject(phoneNumber);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url=apiUrl+"/"+professorId.ToString()+"/"+contactId.ToString()+"/"+"phoneNumber";
            var result = await httpClient.PostAsync(url, data);
        }
    }
}
