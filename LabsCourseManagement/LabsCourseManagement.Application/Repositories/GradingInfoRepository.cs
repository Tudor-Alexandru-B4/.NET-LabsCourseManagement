using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;
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

        public async void Add(GradingInfo gradingInfo)
        {
            await context.GradingInfos.AddAsync(gradingInfo);
        }

        public async Task<List<GradingInfo>> GetAll()
        {
            return await context.GradingInfos.ToListAsync();
        }

        public async Task<GradingInfo?> Get(Guid id)
        {
            return await context.GradingInfos.FirstOrDefaultAsync(c => c.Id == id);
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
