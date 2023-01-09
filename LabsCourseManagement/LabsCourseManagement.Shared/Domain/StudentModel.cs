namespace LabsCourseManagement.Shared.Domain
{
    public class StudentModel
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public bool IsActive { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Email { get; set; }
    }
}
