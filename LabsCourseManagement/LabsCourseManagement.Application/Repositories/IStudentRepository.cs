using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        void Delete(Student student);
        Task<List<Student>> GetAll();
        Task<Student> Get(Guid id);
        void Save();
        void ChangeGroup(Student student, string newGroup);
    }
}