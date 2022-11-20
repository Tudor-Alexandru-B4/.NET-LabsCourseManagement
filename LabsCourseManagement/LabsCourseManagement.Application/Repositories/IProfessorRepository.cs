using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IProfessorRepository
    {
        void Add(Professor professor);
        void Delete(Professor professor);
        List<Professor> GetAll();
        Professor GetById(Guid id);
        void Save();
    }
}