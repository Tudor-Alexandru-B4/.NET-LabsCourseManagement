using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IDatabaseContext context;

        public CatalogRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(Catalog catalog)
        {
            await context.Catalogs.AddAsync(catalog);
        }

        public async Task<List<Catalog>> GetAll()
        {
            return await context.Catalogs.Include(s => s.StudentGrades).ToListAsync();
        }

        public async Task<Catalog> Get(Guid id)
        {
            return await context.Catalogs.Include(s => s.StudentGrades).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Delete(Catalog catalog)
        {
            context.Catalogs.Remove(catalog);
        }

        public void Save()
        {
            context.Save();
        }

    }
}
