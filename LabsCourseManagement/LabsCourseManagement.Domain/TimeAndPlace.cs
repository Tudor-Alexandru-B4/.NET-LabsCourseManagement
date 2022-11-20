namespace LabsCourseManagement.Domain
{
    public class TimeAndPlace
    {
        public Guid Id { get; private set; }
        public DateTime DateAndTime { get; private set; }
        public string Classroom { get; private set; }
    }
}
