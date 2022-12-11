using LabsCourseManagement.Application;
using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly IDatabaseContext context;

        public ProfessorRepository(IDatabaseContext context)
        {
            this.context = context;
        }
        public async void Add(Professor professor)
        {
            await context.Professors.AddAsync(professor);
        }
        public void Delete(Professor professor)
        {
            context.Professors.Remove(professor);
        }
        public async Task<Professor?> GetById(Guid id)
        {
            return await context.Professors.Include(c => c.Courses)
                .Include(l => l.Laboratories).Include(contact => contact.ContactInfo).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Professor>> GetAll()
        {
            return await context.Professors.Include(c => c.Courses)
                .Include(l => l.Laboratories).Include(contact => contact.ContactInfo).ToListAsync();
        }
        public void Save()
        {
            context.Save();
        }
    }
}
