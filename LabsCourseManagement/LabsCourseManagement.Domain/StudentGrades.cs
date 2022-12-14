using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class StudentGrades
    {
        public Guid Id { get; private set; }
        public Student? Student { get; private set; }
        public List<Grade>? Grades { get; private set; }
        public Grade? FinalGrade { get; private set; } = null;

        public static Result<StudentGrades> Create(Student student)
        {
            if (student == null)
            {
                return Result<StudentGrades>.Failure("The student cannot be null");
            }

            var studentGrades = new StudentGrades
            {
                Id = Guid.NewGuid(),
                Student = student,
                Grades = new List<Grade>(),
                FinalGrade = null,
            };

            return Result<StudentGrades>.Success(studentGrades);
        }

        public Result AddGrade(Grade grade)
        {
            if (grade == null)
            {
                return Result.Failure("The grade cannot be null");
            }
            Grades?.Add(grade);
            return Result.Success();
        }

        public Result RemoveGrade(Grade grade)
        {
            if (grade == null)
            {
                return Result.Failure("The grade cannot be null");
            }
            Grades?.Remove(grade);
            return Result.Success();
        }

        public Result ComputeFinalGrade()
        {
            if (Grades?.Count == 0 || Grades == null)
            {
                return Result.Failure("Cannot compute final grade without any grades");
            }

            double sum = Grades.Sum(x => x.Mark);


            FinalGrade = Grade.Create(DateTime.Now, sum / Grades.Count, ExaminationType.Other, "Final Grade").Entity;
            return Result.Success();
        }
    }
}
