namespace LabsAndCourseManagement.Business
{
    public class Grade
    {
        public Guid Id { get; private set; }
        public DateTime GradingDate { get; private set; }
        public double Mark { get; private set; }
        public ExaminationType GradeType { get; private set; }
        public string Mentions { get; private set; }
    }
}