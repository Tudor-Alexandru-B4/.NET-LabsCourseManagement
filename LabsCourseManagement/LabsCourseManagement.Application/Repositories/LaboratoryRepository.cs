﻿using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class LaboratoryRepository : ILaboratoryRepository
    {
        private readonly IDatabaseContext context;

        public LaboratoryRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(Laboratory laboratory)
        {
            await context.Laboratories.AddAsync(laboratory);
        }

        public async Task<List<Laboratory>> GetAll()
        {
            return await context.Laboratories.Include(s => s.LaboratoryStudents)
                .Include(a => a.LaboratoryAnnouncements)
                .Include(g => g.LaboratoryGradingInfo)
                .Include(t => t.LaboratoryTimeAndPlace)
                .Include(c => c.LaboratoryCatalog)
                .Include(c => c.Course)
                .Include(p => p.LaboratoryProfessor).ToListAsync();
        }

        public async Task<Laboratory?> Get(Guid id)
        {
            return await context.Laboratories.Include(s => s.LaboratoryStudents)
                .Include(a => a.LaboratoryAnnouncements)
                .Include(g => g.LaboratoryGradingInfo)
                .Include(t => t.LaboratoryTimeAndPlace)
                .Include(c => c.LaboratoryCatalog)
                .Include(c => c.Course)
                .Include(p => p.LaboratoryProfessor).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Delete(Laboratory laboratory)
        {
            context.Laboratories.Remove(laboratory);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
