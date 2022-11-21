using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Announcement
    {
        public Guid Id { get; private set; }
        public string Header { get; private set; }
        public string Text { get; private set; }
        public DateTime PostingDate { get; private set; }
        public Professor Writer { get; private set; }
        public static Result<Announcement> Create(string header, string text, Professor professor)
        {
            if(header == null || text==null || professor==null)
            {
                return Result<Announcement>.Failure("Header/text/professor cannot be null");
            }
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Header = header,
                Text = text,
                PostingDate = DateTime.Now,
                Writer = professor
            };
            return Result<Announcement>.Success(announcement);
        }
    }
}
