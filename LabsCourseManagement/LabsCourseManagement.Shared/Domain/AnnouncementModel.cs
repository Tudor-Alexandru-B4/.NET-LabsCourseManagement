namespace LabsCourseManagement.Shared.Domain
{
    public class AnnouncementModel
    {
        public string? Header { get; set; }
        public string? Text { get; set; }
        public ProfessorModel? Writer { get; set; }
        public DateTime PostingDate { get; set; }
    }
}
