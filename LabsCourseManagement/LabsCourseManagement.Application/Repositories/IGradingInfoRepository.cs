using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IGradingInfoRepository
    {
        void Add(GradingInfo gradingInfo);
        void Delete(GradingInfo gradingInfo);
        Task<GradingInfo?> Get(Guid id);
        Task<List<GradingInfo>> GetAll();
        void Save();
    }
}