using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class StudentGradesRepository : IStudentGradesRepository
    {
        private readonly IDatabaseContext context;

        public StudentGradesRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(StudentGrades studentGrades)
        {
            context.StudentGrades.Add(studentGrades);
        }

        public List<StudentGrades> GetAll()
        {
            return context.StudentGrades.Include(g => g.Grades).ToList();
        }

        public StudentGrades Get(Guid id)
        {
            return context.StudentGrades.Include(g => g.Grades).FirstOrDefault(c => c.Id == id);
        }

        public void Delete(StudentGrades studentGrades)
        {
            context.StudentGrades.Remove(studentGrades);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
