using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.ExaminationTypes
{
    public interface IExaminationTypeRepository
    {
        void Add(ExaminationType examinationType);
        void Delete(ExaminationType examinationType);
        ExaminationType Get(Guid id);
        List<ExaminationType> GetAll();
        void Save();
    }
}