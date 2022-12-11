using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<ProfessorDto>? Professors { get; set; }
        public List<StudentDto>? Students { get; set; }
    }
}
