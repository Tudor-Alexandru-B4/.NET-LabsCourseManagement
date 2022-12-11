using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class TimeAndPlaceRepository : ITimeAndPlaceRepository
    {
        private readonly IDatabaseContext context;

        public TimeAndPlaceRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(TimeAndPlace timeAndPlace)
        {
            await context.TimesAndPlaces.AddAsync(timeAndPlace);
        }

        public void Delete(TimeAndPlace timeAndPlace)
        {
            context.TimesAndPlaces.Remove(timeAndPlace);
        }

        public async Task<List<TimeAndPlace>> GetAll()
        {
            return await context.TimesAndPlaces.ToListAsync();
        }

        public async Task<TimeAndPlace?> Get(Guid id)
        {
            return await context.TimesAndPlaces.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Boolean Exists(DateTime time, string place)
        {
            return context.TimesAndPlaces.Where(t => t.DateAndTime == time && t.Classroom == place).Any();
        }

        public void Save()
        {
            context.Save();
        }

    }
}
