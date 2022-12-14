using LabsCourseManagement.Domain.Helpers;
namespace LabsCourseManagement.Domain
{
    public class Professor
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        public Contact? ContactInfo { get; private set; }
        public bool IsActive { get; private set; }
        public List<Course>? Courses { get; private set; }
        public List<Laboratory>? Laboratories { get; private set; }

        public static Result<Professor> Create(string name, string surname, string phoneNumber)
        {
            if (name == null || surname == null)
            {
                return Result<Professor>.Failure("Name or surname cannot be null");
            }
            var professor = new Professor
            {
                Id = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                ContactInfo = Contact.Create(phoneNumber).Entity,
                Laboratories = new List<Laboratory>(),
                Courses = new List<Course>(),
                IsActive = true,

            };
            return Result<Professor>.Success(professor);

        }
        public Result AddCourses(List<Course> courses)
        {
            if (!courses.Any())
            {
                return Result.Failure("Courses can not be null");
            }
            courses.ForEach(course => Courses?.Add(course));
            return Result.Success();
        }
        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (!laboratories.Any())
            {
                return Result.Failure("Laboratories can not be null");
            }
            laboratories.ForEach(Laboratory => Laboratories?.Add(Laboratory));
            return Result.Success();
        }
        public Result UpdatePhoneNumber(string phoneNumber)
        {
            var result = ContactInfo?.UpdateContact(phoneNumber);

            if (result == null || result.IsFailure)
            {
                return Result.Failure("Invalid phone number format");
            }

            return Result.Success();
        }
        public Result UpdateName(string name)
        {
            if (name == null)
            {
                return Result.Failure("Name cannot be null");
            }
            Name = name;
            return Result.Success();
        }
        public Result UpdateSurname(string surname)
        {
            if (surname == null)
            {
                return Result.Failure("Surname cannot be null");
            }
            Surname = surname;
            return Result.Success();
        }
        public Result RemoveCourse(Course course)
        {
            if(Courses != null && Courses.Contains(course))
            {
                Courses.Remove(course);
                return Result.Success();
            }
            else
            {
                return Result.Failure("This course does not belong to this professor");
            }
        }
        public Result RemoveLaboratory(Laboratory laboratory)
        {
            if(Laboratories != null && Laboratories.Contains(laboratory))
            {
                Laboratories.Remove(laboratory);
                return Result.Success();
            }
            else
            {
                return Result.Failure("This laboratory does not belong to this professor");
            }
        }
    }
}