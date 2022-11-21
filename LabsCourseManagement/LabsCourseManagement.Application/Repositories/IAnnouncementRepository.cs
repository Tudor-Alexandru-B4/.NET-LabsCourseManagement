using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IAnnouncementRepository
    {
        void Add(Announcement announcement);
        void Delete(Announcement announcement);
        List<Announcement> GetAll();
        Announcement GetById(Guid id);
        void Save();
    }
}