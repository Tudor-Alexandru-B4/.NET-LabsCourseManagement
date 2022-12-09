using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ILaboratoryRepository
    {
        void Add(Laboratory laboratory);
        void Delete(Laboratory laboratory);
        Task<Laboratory> Get(Guid id);
        Task<List<Laboratory>> GetAll();
        void Save();
    }
}