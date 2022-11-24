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

        public static Result<Laboratory> Create(string name, Guid courseId, Professor laboratoryProfessor, string classroom)
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
                LaboratoryProfessor = laboratoryProfessor,
                LaboratoryStudents = new List<Student>(),
                LaboratoryTimeAndPlace = TimeAndPlace.Create(classroom).Entity,
                LaboratoryAnnouncements = new List<Announcement>(),
                LaboratoryGradingInfo = new List<GradingInfo>(),
            };

            return Result<Laboratory>.Success(laboratory);
        }

        public Result AddStudents(List<Student> students)
        {
            if (!students.Any())
            {
                return Result.Failure("Students cannot be null");
            }

            students.ForEach(student => LaboratoryStudents.Add(student));
            return Result.Success();
        }

        public Result AddLaboratoryAnnouncements(List<Announcement> announcements)
        {
            if (!announcements.Any())
            {
                return Result.Failure("Announcements cannot be null");
            }

            announcements.ForEach(announcement => LaboratoryAnnouncements.Add(announcement));
            return Result.Success();
        }

        public Result AddLaboratoryGradingInfos(List<GradingInfo> gradingInfos)
        {
            if (!gradingInfos.Any())
            {
                return Result.Failure("GradingInfos cannot be null");
            }

            gradingInfos.ForEach(gradingInfo => LaboratoryGradingInfo.Add(gradingInfo));
            return Result.Success();
        }
    }
}
