using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Laboratory
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Course Course { get; private set; }
        public bool IsActive { get; private set; }
        public Catalog LaboratoryCatalog { get; private set; }
        public Professor LaboratoryProfessor { get; private set; }
        public List<Student> LaboratoryStudents { get; private set; }
        public TimeAndPlace LaboratoryTimeAndPlace { get; private set; }
        public List<Announcement> LaboratoryAnnouncements { get; private set; }
        public List<GradingInfo> LaboratoryGradingInfo { get; private set; }

        public static Result<Laboratory> Create(string name, Guid courseId)
        {
            if (name == null)
            {
                return Result<Laboratory>.Failure("Name cannot be null");
            }

            if (courseId == null)
            {
                return Result<Laboratory>.Failure("Course Id cannot be null");
            }

            var laboratory = new Laboratory
            {
                Id = Guid.NewGuid(),
                Name = name,
                LaboratoryCatalog = Catalog.Create().Entity,
                IsActive = true,
                LaboratoryProfessor = Professor.Create("FirstName", "Surname").Entity,
                LaboratoryStudents = new List<Student>(),
                LaboratoryTimeAndPlace = null,
                LaboratoryAnnouncements = new List<Announcement>(),
                LaboratoryGradingInfo = new List<GradingInfo>(),
            };

            return Result<Laboratory>.Success(laboratory);
        }
    }
}
