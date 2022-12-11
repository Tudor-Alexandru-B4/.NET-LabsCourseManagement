namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateCourseDto
    {
        public string? Name { get; set; }
        public Guid ProfessorId { get; set; }
    }
}
