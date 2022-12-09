using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ICourseRepository
    {
        void Add(Course course);
        void Delete(Course course);
        Task<Course> Get(Guid id);
        Task<List<Course>> GetAll();
        void Save();
    }
}