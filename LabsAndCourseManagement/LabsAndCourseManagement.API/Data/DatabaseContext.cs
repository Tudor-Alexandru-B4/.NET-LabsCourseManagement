using LabsAndCourseManagement.Business;
using Microsoft.EntityFrameworkCore;

namespace LabsAndCourseManagement.API.Data
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public void Save()
        {
            SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = LabsAndCourseManagement.db");
        }

    }
}
