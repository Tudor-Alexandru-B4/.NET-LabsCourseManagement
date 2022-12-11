using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IProfessorRepository
    {
        void Add(Professor professor);
        void Delete(Professor professor);
        Task<List<Professor>> GetAll();
        Task<Professor?> GetById(Guid id);
        void Save();
    }
}