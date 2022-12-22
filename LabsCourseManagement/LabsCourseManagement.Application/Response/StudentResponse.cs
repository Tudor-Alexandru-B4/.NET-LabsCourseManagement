using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Response
{
    public class StudentResponse
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public string? RegistrationNumber { get; set; }
        public List<Course>? Courses { get; set; }
        public List<Laboratory>? Laboratories { get; set; }
    }
}
