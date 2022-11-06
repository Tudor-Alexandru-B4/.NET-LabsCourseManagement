namespace LabsAndCourseManagement.Business
{
    public class GradingInfo
    {
        public Guid Id { get; private set; }
        public ExaminationType ExaminationType { get; private set; }
        public bool IsMandatory { get; private set; }
        public double MinGrade { get; private set; }
        public double MaxGrade { get; private set; }
        public string Description { get; private set; }
        public TimeAndPlace TimeAndPlace { get; private set; }
    }
}