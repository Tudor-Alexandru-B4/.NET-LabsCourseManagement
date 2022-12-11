namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateLaboratoryDto
    {
        public string? Name { get; set; }
        public Guid CourseId { get; set; }
        public Guid ProfessorId { get; set; }
        public string? DateTime { get; set; }
        public string? Place { get; set; }
    }
}
