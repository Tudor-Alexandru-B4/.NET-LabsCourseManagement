using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class InformationStringRepository : IInformationStringRepository
    {
        private readonly IDatabaseContext databaseContext;

        public InformationStringRepository(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async void Add(InformationString informationString)
        {
            await databaseContext.MyStrings.AddAsync(informationString);
        }
        public void Delete(InformationString informationString)
        {
            databaseContext.MyStrings.Remove(informationString);
        }
        public async Task<InformationString?> GetById(Guid id)
        {
            return await databaseContext.MyStrings.FindAsync(id);
        }
        public async Task<List<InformationString>> GetAll()
        {
            return await databaseContext.MyStrings.ToListAsync();
        }
        public void Save()
        {
            databaseContext.Save();
        }
    }
}
