namespace LabsCourseManagement.WebUI.Dtos
{
    public class TimeAndPlaceDto
    {
        public Guid Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public string? Classroom { get; set; }
    }
}
