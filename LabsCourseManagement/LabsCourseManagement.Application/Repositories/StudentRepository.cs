using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDatabaseContext context;

        public StudentRepository(IDatabaseContext context)
        {
            this.context = context;
        }
        public void Add(Student student)
        {
            context.Students.Add(student);
        }

        public void Delete(Student student)
        {
            context.Students.Remove(student);
        }

        public List<Student> GetAll()
        {
            return context.Students.Include(c => c.Courses)
                .Include(l => l.Laboratories).ToList();
        }

        public Student Get(Guid id)
        {
            return context.Students.Include(c => c.Courses)
                .Include(l => l.Laboratories)
                .FirstOrDefault(s => s.StudentId == id);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
