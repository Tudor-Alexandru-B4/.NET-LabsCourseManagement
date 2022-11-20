using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ICatalogRepository
    {
        void Add(Catalog catalog);
        void Delete(Catalog catalog);
        Catalog Get(Guid id);
        List<Catalog> GetAll();
        void Save();
    }
}