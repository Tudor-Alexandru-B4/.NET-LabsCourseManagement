namespace LabsCourseManagement.Shared.Domain
{
    public class ProfessorModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ContactModel? ContactInfo { get; set; }
        public bool IsActive { get; set; }
    }
}
