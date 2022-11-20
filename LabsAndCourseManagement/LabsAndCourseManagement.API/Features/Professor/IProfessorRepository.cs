using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.Laboratories
{
    public interface IProfessorRepository
    {
        void Add(Professor professor);
        void Delete(Professor professor);
        List<Professor> GetAll();
        Professor GetById(Guid id);
        void Save();
    }
}