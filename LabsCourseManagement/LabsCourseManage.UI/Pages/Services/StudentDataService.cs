//using LabsCourseManagement.Shared;
//using System.Text.Json;
//using System;

//namespace LabsCourseManage.UI.Pages.Services
//{
//    public class StudentDataService : IStudentDataService
//    {
//        private const string ApiURL = "https://localhost:7200/v1/api/students";
//        private readonly HttpClient httpClient;

//        public StudentDataService(HttpClient httpClient)
//        {
//            this.httpClient = httpClient;
//        }
//        public async Task<IEnumerable<Student>> GetAllStudent()
//        {
//            return await JsonSerializer
//                .DeserializeAsync<IEnumerable<Student>> 
//                (await httpClient.GetStreamAsync(ApiURL),
//                new JsonSerializerOptions()
//                {
//                    PropertyNameCaseInsensitive = true,
//                });
//        }

//        public async Task<Student> GetStudentDetail(Guid studentId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
