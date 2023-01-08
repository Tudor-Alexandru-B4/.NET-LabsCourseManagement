using LabsCourseManagement.Shared.Domain;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LabsCourseManagement.UI.Pages.Services
{
    public class StudentDataService : IStudentDataService
    {
        private readonly string apiURL = UrlString.StudentsUrl;
        private readonly HttpClient httpClient;

        public StudentDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateStudent(StudentCreateModel student)
        {
            if (student.Name == null || student.Surname == null || student.Group == null || student.PhoneNumber == null || student.RegistrationNumber == null)
            {
                return;
            }

            JsonObject obj = new JsonObject();
            obj.Add("name", student.Name.ToString());
            obj.Add("surname", student.Surname.ToString());
            obj.Add("year", student.Year);
            obj.Add("group", student.Group.ToString());
            obj.Add("phoneNumber", student.PhoneNumber.ToString());
            obj.Add("registrationNumber", student.RegistrationNumber.ToString());

            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(apiURL, content);
        }

        public async Task DeleteStudent(Guid studentId)
        {
            JsonObject obj = new JsonObject();
            obj.Add("studentId", studentId.ToString());
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}";
            await httpClient.PostAsync(url, content);
            await httpClient.DeleteAsync(url);
        }

        public async Task UpdateName(Guid studentId, string name)
        {
            var json = JsonConvert.SerializeObject(name);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}/name";
            await httpClient.PostAsync(url, data);
        }
        public async Task UpdateSurname(Guid studentId, string surname)
        {
            var json = JsonConvert.SerializeObject(surname);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}/surname";
            await httpClient.PostAsync(url, data);
        }

        public async Task UpdateGroup(Guid studentId, string groupName)
        {
            var json = JsonConvert.SerializeObject(groupName);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}/group";
            await httpClient.PostAsync(url, data);
        }
        public async Task UpdateYear(Guid studentId, int year)
        {
            var json = JsonConvert.SerializeObject(year);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}/year";
            await httpClient.PostAsync(url, data);
        }
        public async Task UpdateRegistrationNumber(Guid studentId, string registrationNumber)
        {
            var json = JsonConvert.SerializeObject(registrationNumber);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{apiURL}/{studentId}/registrationNumber";
            await httpClient.PostAsync(url, data);
        }


        public async Task<IEnumerable<StudentModel>?> GetAllStudents()
        {
            return await System.Text.Json.JsonSerializer
                .DeserializeAsync<IEnumerable<StudentModel>>
                (await httpClient.GetStreamAsync(apiURL),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
        }


    }
}
