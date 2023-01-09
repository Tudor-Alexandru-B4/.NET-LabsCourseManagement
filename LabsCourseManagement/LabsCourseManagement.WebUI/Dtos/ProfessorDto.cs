using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class ProfessorDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public List<CourseDto>? Courses { get; set; }
        public List<LaboratoryDto>? Laboratories { get; set; }
        public ContactDto? ContactInfo { get; set; }
    }
}
