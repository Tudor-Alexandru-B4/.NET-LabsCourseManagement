using LabsCourseManagement.Application;
using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IDatabaseContext context;

        public CourseRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(Course course)
        {
            await context.Courses.AddAsync(course);
        }

        public async Task<List<Course>> GetAll()
        {
            return await context.Courses.Include(p => p.Professors)
                .Include(l => l.Laboratories)
                .Include(s => s.Students)
                .Include(t => t.CourseProgram)
                .Include(a => a.CourseAnnouncements)
                .Include(g => g.CourseGradingInfo)
                .Include(h => h.HelpfulMaterials).ToListAsync();
        }

        public async Task<Course> Get(Guid id)
        {
            return await context.Courses.Include(p => p.Professors)
                .Include(l => l.Laboratories)
                .Include(s => s.Students)
                .Include(t => t.CourseProgram)
                .Include(a => a.CourseAnnouncements)
                .Include(g => g.CourseGradingInfo)
                .Include(h => h.HelpfulMaterials).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Delete(Course course)
        {
            context.Courses.Remove(course);
        }

        public void Save()
        {
            context.Save();
        }

    }
}
