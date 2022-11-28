using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

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
            return context.Contacts.Include(e => e.EmailAddresses).ToList();
        }

        public Contact Get(Guid id)
        {
            return context.Contacts.Include(e => e.EmailAddresses).FirstOrDefault(c => c.Id == id);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
