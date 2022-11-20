using LabsCourseManagement.Application;
using LabsCourseManagement.Domain;

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
            Console.WriteLine("WHY?!?!?!");
            context.Courses.Add(course);
            Console.WriteLine("WHY?!?!?!");
        }

        public List<Course> GetAll()
        {
            return context.Courses.ToList();
        }

        public Course Get(Guid id)
        {
            return context.Courses.FirstOrDefault(c => c.Id == id);
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
