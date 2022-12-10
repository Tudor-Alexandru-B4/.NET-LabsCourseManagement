using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IGradeRepository
    {
        void Add(Grade grade);
        void Delete(Grade grade);
        Task<Grade> Get(Guid id);
        Task<List<Grade>> GetAll();
        void Save();
    }
}