using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IContactRepository
    {
        void Add(Contact contact);
        void Delete(Contact contact);
        Task<Contact> Get(Guid id);
        Task<List<Contact>> GetAll();
        void Save();
    }
}