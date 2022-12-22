using LabsCourseManagement.Application.Response;
using MediatR;

namespace LabsCourseManagement.Application.Queries
{
    public class CreateStudentRequest : IRequest<StudentResponse>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
