using LabsCourseManagement.Application;
using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Infrastructure
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Announcement> Announcements => Set<Announcement>();
        public DbSet<Catalog> Catalogs => Set<Catalog>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Grade> Grades => Set<Grade>();
        public DbSet<GradingInfo> GradingInfos => Set<GradingInfo>();
        public DbSet<Laboratory> Laboratories => Set<Laboratory>();
        public DbSet<MyString> MyStrings => Set<MyString>();
        public DbSet<Professor> Professors => Set<Professor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudentGrades> StudentGrades => Set<StudentGrades>();
        public DbSet<TimeAndPlace> TimesAndPlaces => Set<TimeAndPlace>();

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = LabsCourseManagement.db");
        }
    }
}
