using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateTimeAndPlaceDto
    {
        public string? Classroom { get; set; }
        public string DateTime { get; set; }
    }
}
