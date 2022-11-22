using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IGradingInfoRepository
    {
        void Add(GradingInfo gradingInfo);
        void Delete(GradingInfo gradingInfo);
        GradingInfo Get(Guid id);
        List<GradingInfo> GetAll();
        void Save();
    }
}