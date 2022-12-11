namespace LabsCourseManagement.WebUI.Dtos
{
    public class StudentDto
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public string? RegistrationNumber { get; set; }
        public List<CourseDto>? Courses { get; set; }
        public List<LaboratoryDto>? Laboratories { get; set; }
    }
}
