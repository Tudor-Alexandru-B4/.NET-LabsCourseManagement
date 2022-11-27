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
        public void Add(Professor professor)
        {
            context.Professors.Add(professor);
        }
        public void Delete(Professor professor)
        {
            context.Professors.Remove(professor);
        }
        public Professor GetById(Guid id)
        {
            return context.Professors.Find(id);
        }
        public List<Professor> GetAll()
        {
            return context.Professors.ToList();
        }
        public void Save()
        {
            context.Save();
        }
    }
}
