namespace LabsAndCourseManagement.Business
{
    public class GradingInfo
    {
        Guid Id { get; set; }
        ExaminationType ExaminationType { get; set; }
        Boolean IsMandatory { get; set; }
        Double MinGrade { get; set; }
        Double MaxGrade { get; set; }
        String Description { get; set; }
        TimeAndPlace TimeAndPlace { get; set; }
    }
}