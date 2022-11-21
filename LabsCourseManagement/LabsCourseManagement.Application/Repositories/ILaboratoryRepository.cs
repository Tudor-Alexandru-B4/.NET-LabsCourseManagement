using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ILaboratoryRepository
    {
        void Add(Laboratory laboratory);
        void Delete(Laboratory laboratory);
        Laboratory Get(Guid id);
        List<Laboratory> GetAll();
        void Save();
    }
}