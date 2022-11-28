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

        public static Result<Student> Create(string name, string surname, string group, int year, string registrationNumber, string phoneNumber)
        {
            if (name == null || surname == null || group == null || year <= 0 || registrationNumber == null || phoneNumber == null)
            {
                return Result<Student>.Failure("The field cannot be null / year cannot be less than 1");
            }

            var student = new Student
            {
                StudentId = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                ContactInfo = Contact.Create(phoneNumber).Entity,
                Group = group,
                Year = year,
                IsActive = true,
                RegistrationNumber = registrationNumber,
                Courses = new List<Course>(),
                Laboratories = new List<Laboratory>()
            };

            return Result<Student>.Success(student);
        }

        public Result AddCourse(List<Course> courses)
        {
            if (courses.Any(course => course == null))
            {
                return Result.Failure("Courses cannot be null");
            }

            courses.ForEach(course => Courses.Add(course));
            return Result.Success();
        }

        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (laboratories.Any(laboratory => laboratory == null))
            {
                return Result.Failure("Laboratories cannot be null");
            }

            laboratories.ForEach(lab => Laboratories.Add(lab));
            return Result.Success();
        }
    }


}
