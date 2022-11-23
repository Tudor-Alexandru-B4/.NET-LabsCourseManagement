using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IContactRepository
    {
        void Add(Contact contact);
        void Delete(Contact contact);
        Contact Get(Guid id);
        List<Contact> GetAll();
        void Save();
    }
}