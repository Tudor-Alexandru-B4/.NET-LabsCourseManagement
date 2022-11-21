using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly IDatabaseContext databaseContext;

        public AnnouncementRepository(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public void Add(Announcement announcement)
        {
            databaseContext.Announcements.Add(announcement);
        }
        public void Delete(Announcement announcement)
        {
            databaseContext.Announcements.Remove(announcement);
        }
        public Announcement GetById(Guid id)
        {
            return databaseContext.Announcements.Find(id);
        }
        public List<Announcement> GetAll()
        {
            return databaseContext.Announcements.Include(a => a.Writer).ToList();
            //return context.Announcements.ToList();
        }
        public void Save()
        {
            databaseContext.Save();
        }
    }
}
