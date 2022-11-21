using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public class LaboratoryRepository : ILaboratoryRepository
    {
        private readonly IDatabaseContext context;

        public LaboratoryRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(Laboratory laboratory)
        {
            context.Laboratories.Add(laboratory);
        }

        public List<Laboratory> GetAll()
        {
            return context.Laboratories.ToList();
        }

        public Laboratory Get(Guid id)
        {
            return context.Laboratories.FirstOrDefault(c => c.Id == id);
        }

        public void Delete(Laboratory laboratory)
        {
            context.Laboratories.Remove(laboratory);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
