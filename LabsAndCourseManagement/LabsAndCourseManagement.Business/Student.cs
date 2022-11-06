namespace LabsAndCourseManagement.Business

{
    public class Student
    {
        public Guid StudentId { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Contact ContactInfo { get; private set; }
        public int Year { get; private set; }
        public string Group { get; private set; }
        public bool IsActive { get; private set; }
        public string RegistrationNumber { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<Laboratory> Laboratories { get; private set; }
    }

}
