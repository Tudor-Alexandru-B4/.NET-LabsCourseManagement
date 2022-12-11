using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public DateTime PostingDate { get; set; }
        public Professor? Writer { get; set; }
    }
}
