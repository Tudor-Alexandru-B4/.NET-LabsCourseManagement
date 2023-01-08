using LabsCourseManagement.Domain.Helpers;
using MediatR;

namespace LabsCourseManagement.Domain

{
    public class Student
    {
        public Guid StudentId { get; private set; }
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        public Contact? ContactInfo { get; private set; }
        public int Year { get; private set; }
        public string? Group { get; set; }
        public bool IsActive { get; private set; }
        public string? RegistrationNumber { get; private set; }
        public List<Course>? Courses { get; private set; }
        public List<Laboratory>? Laboratories { get; private set; }

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

            courses.ForEach(course => Courses?.Add(course));
            return Result.Success();
        }

        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (laboratories.Any(laboratory => laboratory == null))
            {
                return Result.Failure("Laboratories cannot be null");
            }

            laboratories.ForEach(lab => Laboratories?.Add(lab));
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
                return Result.Failure("Name cannot be null");
            }
            Surname = surname;
            return Result.Success();
        }
        public Result UpdateGroup(string newGroup)
        {
            if (newGroup == null)
            {
                return Result.Failure("Group cannot be null");
            }
            Group = newGroup;
            return Result.Success();
        }
        public Result UpdateYear(int year)
        {
            if (year <= 0)
            {
                return Result.Failure("Year cannot be less then 1");
            }
            Year = year;
            return Result.Success();
        }
        public Result UpdateRegistrationNumber(string registrationNumber)
        {
            if (registrationNumber == null)
            {
                return Result.Failure("Registration number cannot be null");
            }
            RegistrationNumber = registrationNumber;
            return Result.Success();
        }


    }


}
