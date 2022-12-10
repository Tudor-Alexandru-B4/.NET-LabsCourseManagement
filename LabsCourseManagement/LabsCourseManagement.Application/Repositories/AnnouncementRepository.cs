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
        public async void Add(Announcement announcement)
        {
            await databaseContext.Announcements.AddAsync(announcement);
        }
        public void Delete(Announcement announcement)
        {
            databaseContext.Announcements.Remove(announcement);
        }
        public async Task<Announcement> GetById(Guid id)
        {
            return await databaseContext.Announcements.FindAsync(id);
        }
        public async Task<List<Announcement>> GetAll()
        {
            return await databaseContext.Announcements.Include(a => a.Writer).ToListAsync();
        }
        public void Save()
        {
            databaseContext.Save();
        }
    }
}
