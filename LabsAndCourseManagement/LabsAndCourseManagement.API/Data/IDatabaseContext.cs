using LabsAndCourseManagement.Business;
using Microsoft.EntityFrameworkCore;

namespace LabsAndCourseManagement.API.Data
{
    public interface IDatabaseContext
    {
        DbSet<Announcement> Announcements { get; }
        DbSet<Course> Courses { get; }
        DbSet<Laboratory> Laboratories { get;}
        DbSet<Professor> Professors { get; }
        DbSet<Student> Students { get; }
        DbSet<Catalog> Catalogs { get; }

        void Save();
    }
}