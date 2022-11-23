using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public class TimeAndPlaceRepository : ITimeAndPlaceRepository
    {
        private readonly IDatabaseContext context;

        public TimeAndPlaceRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(TimeAndPlace timeAndPlace)
        {
            context.TimesAndPlaces.Add(timeAndPlace);
        }

        public void Delete(TimeAndPlace timeAndPlace)
        {
            context.TimesAndPlaces.Remove(timeAndPlace);
        }

        public List<TimeAndPlace> GetAll()
        {
            return context.TimesAndPlaces.ToList();
        }

        public TimeAndPlace Get(Guid id)
        {
            return context.TimesAndPlaces.FirstOrDefault(t => t.Id == id);
        }

        public void Save()
        {
            context.Save();
        }

    }
}
