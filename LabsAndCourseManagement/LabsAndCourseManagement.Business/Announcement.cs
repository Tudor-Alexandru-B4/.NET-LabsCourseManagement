namespace LabsAndCourseManagement.Business
{
    public class Announcement
    {
        Guid Id { get; set; }
        String Header { get; set; }
        String Text { get; set; }
        DateTime PostingDate { get; set; }
        Professor Writer { get; set; }
    }
}