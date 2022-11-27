using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Grade
    {
        public Guid Id { get; private set; }
        public DateTime GradingDate { get; private set; }
        public double Mark { get; private set; }
        public ExaminationType GradeType { get; private set; }
        public string Mentions { get; private set; }

        public static Result<Grade> Create(DateTime gradingDate, double mark, ExaminationType type, string mentions)
        {

            if (mark <= 0 || mark > 10)
            {
                return Result<Grade>.Failure("The mark has to be a value greater than 0 and lower or equal to 10");
            }

            var grade = new Grade
            {
                Id = Guid.NewGuid(),
                GradingDate = gradingDate,
                Mark = mark,
                GradeType = type,
                Mentions = mentions,
            };
            return Result<Grade>.Success(grade);
        }
    }
}