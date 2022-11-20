namespace LabsCourseManagement.Domain
{
    public class Announcement
    {
        public Guid Id { get; private set; }
        public string Header { get; private set; }
        public string Text { get; private set; }
        public DateTime PostingDate { get; private set; }
        public Professor Writer { get; private set; }
    }
}
