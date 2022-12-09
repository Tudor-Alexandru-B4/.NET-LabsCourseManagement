using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ICatalogRepository
    {
        void Add(Catalog catalog);
        void Delete(Catalog catalog);
        Task<Catalog> Get(Guid id);
        Task<List<Catalog>> GetAll();
        void Save();
    }
}