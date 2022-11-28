using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

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
            return context.Laboratories.Include(s => s.LaboratoryStudents)
                .Include(a => a.LaboratoryAnnouncements)
                .Include(g => g.LaboratoryGradingInfo).ToList();
        }

        public Laboratory Get(Guid id)
        {
            return context.Laboratories.Include(s => s.LaboratoryStudents)
                .Include(a => a.LaboratoryAnnouncements)
                .Include(g => g.LaboratoryGradingInfo).FirstOrDefault(c => c.Id == id);
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
