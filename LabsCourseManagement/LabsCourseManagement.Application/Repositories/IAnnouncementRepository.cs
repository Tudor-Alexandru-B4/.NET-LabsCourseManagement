using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IAnnouncementRepository
    {
        void Add(Announcement announcement);
        void Delete(Announcement announcement);
        Task<List<Announcement>> GetAll();
        Task<Announcement> GetById(Guid id);
        void Save();
    }
}