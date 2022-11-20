using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.ExaminationTypes
{
    public class ExaminationTypeRepository : IExaminationTypeRepository
    {
        private readonly IDatabaseContext context;

        public ExaminationTypeRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(ExaminationType examinationType)
        {
            context.ExaminationTypes.Add(examinationType);
        }

        public List<ExaminationType> GetAll()
        {
            return context.ExaminationTypes.ToList();
        }

        public ExaminationType Get(Guid id)
        {
            return context.ExaminationTypes.FirstOrDefault(c => c.Id == id);
        }

        public void Delete(ExaminationType examinationType)
        {
            context.ExaminationTypes.Remove(examinationType);
        }

        public void Save()
        {
            context.Save();
        }

    }
}
