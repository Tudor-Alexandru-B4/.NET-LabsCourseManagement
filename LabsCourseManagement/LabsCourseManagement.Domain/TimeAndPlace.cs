using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class TimeAndPlace
    {
        public Guid Id { get; private set; }
        public DateTime DateAndTime { get; private set; }
        public string Classroom { get; private set; }

        public static Result<TimeAndPlace> Create(DateTime dateTime, string classroom)
        {
            if(classroom == null)
            {
                return Result<TimeAndPlace>.Failure("Classroom cannot be null");
            }

            var timeAndPlace = new TimeAndPlace
            {
                Id = Guid.NewGuid(),
                DateAndTime = dateTime,
                Classroom = classroom
            };
            return Result<TimeAndPlace>.Success(timeAndPlace);

        }
    }
}
