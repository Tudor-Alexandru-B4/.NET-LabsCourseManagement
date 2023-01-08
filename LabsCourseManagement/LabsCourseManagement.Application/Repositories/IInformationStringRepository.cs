using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface IInformationStringRepository
    {
        void Add(InformationString informationString);
        void Delete(InformationString informationString);
        Task<InformationString?> GetById(Guid id);
        Task<List<InformationString>> GetAll();
        void Save();
    }
}