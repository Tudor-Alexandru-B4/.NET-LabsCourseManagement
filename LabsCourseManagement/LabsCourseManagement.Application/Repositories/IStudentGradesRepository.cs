using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IStudentGradesRepository
    {
        void Add(StudentGrades studentGrades);
        void Delete(StudentGrades studentGrades);
        StudentGrades Get(Guid id);
        List<StudentGrades> GetAll();
        void Save();
    }
}