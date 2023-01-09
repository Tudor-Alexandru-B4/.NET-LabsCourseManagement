namespace LabsCourseManagement.UI.Pages.InputClasses
{
    public class GradingInput
    {
        public string? ExaminationType { get; set; }
        public bool IsMandatory { get; set; }
        public double MinGrade { get; set; }
        public double MaxGrade { get; set; }
        public string? Description { get; set; }
        public string? Classroom { get; set; }
        public string? DateTime { get; set; }
    }
}
