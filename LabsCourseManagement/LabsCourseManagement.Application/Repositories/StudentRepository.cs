using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDatabaseContext context;

        public StudentRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public async Task<Student> Add(Student student)
        {
            await context.Students.AddAsync(student);
            context.Save();
            return student;
        }
        
        public void Delete(Student student)
        {
            context.Students.Remove(student);
        }

        public async Task<List<Student>> GetAll()
        {
            return await context.Students.Include(c => c.Courses)
                .Include(l => l.Laboratories).ToListAsync();
        }

        public async Task<Student?> Get(Guid id)
        {
            return await context.Students.Include(c => c.Courses)
                .Include(l => l.Laboratories)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public void ChangeGroup(Student student, string newGroup)
        {
            var id = student.StudentId;
            var searchedStudent = context.Students.FirstOrDefault(s => s.StudentId == id);
            if (searchedStudent != null)
            {
                searchedStudent.Group = newGroup;
            }
            
        }

        public void Save()
        {
            context.Save();
        }

      
    }
}
