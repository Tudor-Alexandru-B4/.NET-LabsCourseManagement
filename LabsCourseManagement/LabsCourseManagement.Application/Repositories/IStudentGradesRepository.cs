using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IStudentGradesRepository
    {
        void Add(StudentGrades studentGrades);
        void Delete(StudentGrades studentGrades);
        Task<StudentGrades> Get(Guid id);
        Task<List<StudentGrades>> GetAll();
        void Save();
    }
}