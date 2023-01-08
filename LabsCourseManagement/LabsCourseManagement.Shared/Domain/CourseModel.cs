namespace LabsCourseManagement.Shared.Domain
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public List<ProfessorModel>? Professors { get; set; }
        public List<StudentModel>? Students { get; set; }
    }
}
