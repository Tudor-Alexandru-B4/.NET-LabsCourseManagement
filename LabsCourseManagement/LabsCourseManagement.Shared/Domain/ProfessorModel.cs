using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Shared.Domain
{
    public class ProfessorModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ContactModel ContactInfo { get; set; }
        public bool IsActive { get; set; }
        //public List<Course>? Courses { get; set; }
        //public List<Laboratory>? Laboratories { get; set; }
    }
}
