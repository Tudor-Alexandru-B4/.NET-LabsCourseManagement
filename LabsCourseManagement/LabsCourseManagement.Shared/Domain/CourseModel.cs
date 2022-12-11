namespace LabsCourseManagement.Shared.Domain
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
