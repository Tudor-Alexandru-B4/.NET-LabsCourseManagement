using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.Courses
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