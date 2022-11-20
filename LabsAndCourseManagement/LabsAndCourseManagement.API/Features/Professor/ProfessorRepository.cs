using LabsAndCourseManagement.API.Data;
using LabsAndCourseManagement.Business;
using System.Security.Cryptography.X509Certificates;

namespace LabsAndCourseManagement.API.Features.Laboratories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly DatabaseContext context;

        public ProfessorRepository(DatabaseContext context)
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
