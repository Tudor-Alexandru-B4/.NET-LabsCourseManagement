using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LabsCourseManagement.Application.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly IDatabaseContext context;

        public GradeRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(Grade grade)
        {
            await context.Grades.AddAsync(grade);
        }

        public async Task<List<Grade>> GetAll()
        {
            return await context.Grades.ToListAsync();
        }

        public async Task<Grade> Get(Guid id)
        {
            return await context.Grades.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Delete(Grade grade)
        {
            context.Grades.Remove(grade);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
