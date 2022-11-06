namespace LabsAndCourseManagement.Business
{
    public class Grade
    {
        public Guid Id { get; private set; }
        public DateTime GradingDate { get; private set; }
        public Double Mark { get; private set; }
        public ExaminationType GradeType { get; private set; }
        public String Mentions { get; private set; }
    }
}