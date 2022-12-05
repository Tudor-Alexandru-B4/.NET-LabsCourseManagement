using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        void Delete(Student student);
        List<Student> GetAll();
        Student Get(Guid id);
        void Save();
        void ChangeGroup(Student student, string newGroup);
    }
}