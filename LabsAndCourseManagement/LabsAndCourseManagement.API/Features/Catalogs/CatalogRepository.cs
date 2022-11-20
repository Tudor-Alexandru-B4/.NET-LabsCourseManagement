using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.Catalogs
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IDatabaseContext context;

        public CatalogRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(Catalog catalog)
        {
            context.Catalogs.Add(catalog);
        }

        public List<Catalog> GetAll()
        {
            return context.Catalogs.ToList();
        }

        public Catalog Get(Guid id)
        {
            return context.Catalogs.FirstOrDefault(c => c.Id == id);
        }

        public void Delete(Catalog catalog)
        {
            context.Catalogs.Remove(catalog);
        }

        public void Save()
        {
            context.Save();
        }

    }
}
