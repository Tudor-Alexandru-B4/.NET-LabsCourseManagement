namespace LabsCourseManagement.Shared.Domain
{
    public class TimeAndPlaceModel
    {
        public Guid Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public string? Classroom { get; set; }
    }
}
