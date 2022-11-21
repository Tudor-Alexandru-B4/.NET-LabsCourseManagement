using LabsCourseManagement.Domain;
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

        public void Add(Grade grade)
        {
            context.Grades.Add(grade);
        }

        public List<Grade> GetAll()
        {
            return context.Grades.ToList();
        }

        public Grade Get(Guid id)
        {
            return context.Grades.FirstOrDefault(c => c.Id == id);
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
