using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class TimeAndPlace
    {
        public Guid Id { get; private set; }
        public DateTime DateAndTime { get; private set; }
        public string Classroom { get; private set; }

        public static Result<TimeAndPlace> Create(string classroom)
        {
            var timeAndPlace = new TimeAndPlace
            {
                Id = Guid.NewGuid(),
                DateAndTime = DateTime.Now,
                Classroom = classroom
            };
            return Result<TimeAndPlace>.Success(timeAndPlace);

        }
    }
}
