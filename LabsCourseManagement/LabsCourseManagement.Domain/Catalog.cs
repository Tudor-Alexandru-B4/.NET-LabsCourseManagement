using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Catalog
    {
        public Guid Id { get; private set; }
        public List<StudentGrades> StudentGrades { get; private set; }
    
        public static Result<Catalog> Create()
        {
            var catalog = new Catalog
            {
                Id = Guid.NewGuid(),
                StudentGrades = new List<StudentGrades>()
            };
            return Result<Catalog>.Success(catalog);
        }

        public Result AddStudentGradesToCatalog(List<StudentGrades> studentGrades)
        {
            if(studentGrades.Any(studentGrade => studentGrade == null))
            {
                return Result.Failure("StudentGrades cannot be null");
            }

            studentGrades.ForEach(studentGrades => StudentGrades.Add(studentGrades));
            return Result.Success();
        }
    
    }
}
