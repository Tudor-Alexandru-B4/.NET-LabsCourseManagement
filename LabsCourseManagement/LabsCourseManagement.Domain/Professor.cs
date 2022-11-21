using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Professor
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        public Contact ContactInfo { get; private set; }
        public bool IsActive { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<Laboratory> Laboratories { get; private set; }

        public static Result<Professor> Create(string name, string surname)
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
                ContactInfo = new Contact(),
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
            courses.ForEach(course => Courses.Add(course));
            return Result.Success();
        }
        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (!laboratories.Any())
            {
                return Result.Failure("Laboratories can not be null");
            }
            laboratories.ForEach(Laboratory => Laboratories.Add(Laboratory));
            return Result.Success();
        }
    }
}