using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ICourseRepository
    {
        void Add(Course course);
        void Delete(Course course);
        Course Get(Guid id);
        List<Course> GetAll();
        void Save();
    }
}