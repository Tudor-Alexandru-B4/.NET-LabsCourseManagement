namespace LabsAndCourseManagement.Business
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

        public Result AddStudentGradesToCatalog(StudentGrades studentGrades)
        {
            if(studentGrades == null)
            {
                return Result.Failure("StudentGrades cannot be null");
            }

            StudentGrades.Add(studentGrades);
            return Result.Success();
        }
    
    }
}
