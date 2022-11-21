using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain

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

        public static Result<Student> Create(string name, string surname, string group, int year, string registrationNumber)
        {
            if (name == null || surname == null || group == null || year == null || registrationNumber == null)
            {
                return Result<Student>.Failure("The field cannot be null");
            }

            var student = new Student
            {
                StudentId = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                ContactInfo = new Contact(),
                Group = group,
                Year = year,
                IsActive = true,
                RegistrationNumber = registrationNumber,
                Courses = new List<Course>(),
                Laboratories = new List<Laboratory>()
            };

            return Result<Student>.Success(student);
        }
    }


}
