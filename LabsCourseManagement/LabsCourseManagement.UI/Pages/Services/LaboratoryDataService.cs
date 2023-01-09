using LabsCourseManagement.Shared.Domain;
using LabsCourseManagement.UI.Pages.InputClasses;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace LabsCourseManagement.UI.Pages.Services
{
    public class LaboratoryDataService : ILaboratoryDataService
    {
        private readonly string apiUrl = UrlString.LaboratoryUrl;
        private readonly string apiUrlV2 = UrlString.LaboratoryUrlV2;
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

        public async Task AddAnnouncementsToLaboratory(Guid laboratoryId, List<AnnouncementInput> announcementsInputs)
        {
            await httpClient.PostAsJsonAsync($"{apiUrlV2}/{laboratoryId}/announcements", announcementsInputs);
        }

        public async Task AddGradingsToLaboratory(Guid laboratoryId, List<GradingInput> gradingsInputs)
        {
            await httpClient.PostAsJsonAsync($"{apiUrlV2}/{laboratoryId}/gradings", gradingsInputs);
        }

        public async Task AddStudentsToLaboratory(Guid laboratoryId, List<Guid> studentsIds)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{laboratoryId}/addStudents", studentsIds);
        }

        public async Task CreateLaboratory(LaboratoryInput laboratory)
        {
            JsonObject obj = new JsonObject();
            
            obj.Add("name", laboratory.Name?.ToString());
            obj.Add("courseId", laboratory.CourseId.ToString());
            obj.Add("professorId", laboratory.ProfessorId.ToString());
            obj.Add("dateTime", laboratory.DateTime?.ToString());
            obj.Add("place", laboratory.Place?.ToString());

            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(apiUrl, content);
        }

        public async Task DeleteLaboratory(Guid laboratoryId)
        {
            await httpClient.DeleteAsync($"{apiUrl}/{laboratoryId}");
        }

        public async Task RemoveAnnouncementsFromLaboratory(Guid laboratoryId, List<Guid> announcementsIds)
        {
            await httpClient.PutAsJsonAsync($"{apiUrlV2}/{laboratoryId}/announcements", announcementsIds);
        }

        public async Task RemoveGradingsFromLaboratory(Guid laboratoryId, List<Guid> gradingsIds)
        {
            await httpClient.PutAsJsonAsync($"{apiUrlV2}/{laboratoryId}/gradings", gradingsIds);
        }

        public async Task RemoveStudentsFromLaboratory(Guid laboratoryId, List<Guid> studentsIds)
        {
            await httpClient.PutAsJsonAsync($"{apiUrlV2}/{laboratoryId}/students", studentsIds);
        }

        public async Task UpdateActiveStatus(Guid laboratoryId, bool activeStatus)
        {
            await httpClient.PutAsJsonAsync($"{apiUrlV2}/{laboratoryId}/active", activeStatus);
        }

        public async Task UpdateName(Guid laboratoryId, string name)
        {
            await httpClient.PutAsJsonAsync($"{apiUrlV2}/{laboratoryId}/name", name);
        }

        public async Task UpdateProfessorFromLaboratory(Guid laboratoryId, Guid professorId)
        {
            await httpClient.PostAsJsonAsync($"{apiUrl}/{laboratoryId}/professor", professorId);
        }
    }
}
