using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class StudentGradesRepository : IStudentGradesRepository
    {
        private readonly IDatabaseContext context;

        public StudentGradesRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async void Add(StudentGrades studentGrades)
        {
            await context.StudentGrades.AddAsync(studentGrades);
        }

        public async Task<List<StudentGrades>> GetAll()
        {
            return await context.StudentGrades.Include(g => g.Grades).ToListAsync();
        }

        public async Task<StudentGrades> Get(Guid id)
        {
            return await context.StudentGrades.Include(g => g.Grades).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Delete(StudentGrades studentGrades)
        {
            context.StudentGrades.Remove(studentGrades);
        }

        public void Save()
        {
            context.Save();
        }
    }
}
