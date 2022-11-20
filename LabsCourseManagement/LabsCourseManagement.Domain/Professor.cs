namespace LabsCourseManagement.Domain
{
    public class Professor
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Contact ContactInfo { get; private set; }
        public bool IsActive { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<Laboratory> Laboratories { get; private set; }

    }
}