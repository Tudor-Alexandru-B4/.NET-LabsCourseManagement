using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDatabaseContext context;

        public ContactRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(Contact contact)
        {
            context.Contacts.Add(contact);
        }

        public void Delete(Contact contact)
        {
            context.Contacts.Remove(contact);
        }

        public List<Contact> GetAll()
        {
            return context.Contacts.ToList();
        }

        public Contact Get(Guid id)
        {
            return context.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
