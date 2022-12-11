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

        public async void Add(Contact contact)
        {
            await context.Contacts.AddAsync(contact);
        }

        public void Delete(Contact contact)
        {
            context.Contacts.Remove(contact);
        }

        public async Task<List<Contact>> GetAll()
        {
            return await context.Contacts.Include(e => e.EmailAddresses).ToListAsync();
        }

        public async Task<Contact?> Get(Guid id)
        {
            return await context.Contacts.Include(e => e.EmailAddresses).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
