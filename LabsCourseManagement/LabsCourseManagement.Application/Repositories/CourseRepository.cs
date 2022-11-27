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

        public void Add(Course course)
        {
            context.Courses.Add(course);
        }

        public List<Course> GetAll()
        {
            return context.Courses.Include(p => p.Professors)
                .Include(l => l.Laboratories)
                .Include(s => s.Students)
                .Include(t => t.CourseProgram)
                .Include(a => a.CourseAnnouncements)
                .Include(g => g.CourseGradingInfo)
                .Include(h => h.HelpfulMaterials).ToList();
        }

        public Course Get(Guid id)
        {
            return context.Courses.Include(p => p.Professors)
                .Include(l => l.Laboratories)
                .Include(s => s.Students)
                .Include(t => t.CourseProgram)
                .Include(a => a.CourseAnnouncements)
                .Include(g => g.CourseGradingInfo)
                .Include(h => h.HelpfulMaterials).FirstOrDefault(c => c.Id == id);
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
