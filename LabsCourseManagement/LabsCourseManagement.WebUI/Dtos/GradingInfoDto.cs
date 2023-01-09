using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class GradingInfoDto
    {
        public Guid Id { get; set; }
        public ExaminationType ExaminationType { get; set; }
        public bool IsMandatory { get; set; }
        public double MinGrade { get; set; }
        public double MaxGrade { get; set; }
        public string? Description { get; set; }
        public TimeAndPlaceDto? TimeAndPlace { get; set; }
    }
}
