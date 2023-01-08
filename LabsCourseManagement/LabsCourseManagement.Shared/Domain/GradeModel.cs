namespace LabsCourseManagement.Shared.Domain
{
    public class GradeModel
    {
        public DateTime GradingDate { get; set; }
        public double Mark { get; set; }
        public ExaminationTypeModel GradeType { get; set; }
        public string? Mentions { get; set; }
    }
}
