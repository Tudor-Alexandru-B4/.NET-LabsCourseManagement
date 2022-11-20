using LabsCourseManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabsCourseManagement.Application
{
    public interface IDatabaseContext
    {
        public DbSet<Announcement> Announcements { get; }
        public DbSet<Catalog> Catalogs { get; }
        public DbSet<Contact> Contacts { get; }
        public DbSet<Course> Courses { get; }
        public DbSet<Grade> Grades { get; }
        public DbSet<GradingInfo> GradingInfos { get; }
        public DbSet<Laboratory> Laboratories { get; }
        public DbSet<MyString> MyStrings { get; }
        public DbSet<Professor> Professors { get; }
        public DbSet<Student> Students { get; }
        public DbSet<StudentGrades> StudentGrades { get; }
        public DbSet<TimeAndPlace> TimesAndPlaces { get; }

        public void Save();
    }
}
