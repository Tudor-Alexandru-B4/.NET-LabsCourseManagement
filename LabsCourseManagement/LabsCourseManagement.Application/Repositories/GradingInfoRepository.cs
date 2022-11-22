using LabsCourseManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsCourseManagement.Application.Repositories
{
    public class GradingInfoRepository : IGradingInfoRepository
    {
        private readonly IDatabaseContext context;

        public GradingInfoRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public void Add(GradingInfo gradingInfo)
        {
            context.GradingInfos.Add(gradingInfo);
        }

        public List<GradingInfo> GetAll()
        {
            return context.GradingInfos.ToList();
        }

        public GradingInfo Get(Guid id)
        {
            return context.GradingInfos.FirstOrDefault(c => c.Id == id);
        }

        public void Delete(GradingInfo gradingInfo)
        {
            context.GradingInfos.Remove(gradingInfo);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
