using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IGradeRepository
    {
        void Add(Grade grade);
        void Delete(Grade grade);
        Grade Get(Guid id);
        List<Grade> GetAll();
        void Save();
    }
}