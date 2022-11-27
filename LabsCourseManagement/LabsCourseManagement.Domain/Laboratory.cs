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

        public static Result<Laboratory> Create(string name, Course course, 
            Professor laboratoryProfessor, TimeAndPlace timeAndPlace)
        {
            if (name == null)
            {
                return Result<Laboratory>.Failure("Name cannot be null");
            }

            var laboratory = new Laboratory
            {
                Id = Guid.NewGuid(),
                Name = name,
                Course = course,
                LaboratoryCatalog = Catalog.Create().Entity,
                IsActive = true,
                LaboratoryProfessor = laboratoryProfessor,
                LaboratoryStudents = new List<Student>(),
                LaboratoryTimeAndPlace = timeAndPlace,
                LaboratoryAnnouncements = new List<Announcement>(),
                LaboratoryGradingInfo = new List<GradingInfo>(),
            };

            return Result<Laboratory>.Success(laboratory);
        }

        public Result AddStudents(List<Student> students)
        {
            if (students.Any(student => student == null))
            {
                return Result.Failure("Cannot add null students");
            }

            students.ForEach(student => LaboratoryStudents.Add(student));
            return Result.Success();
        }

        public Result AddLaboratoryAnnouncements(List<Announcement> announcements)
        {
            if (announcements.Any(announcement => announcement == null))
            {
                return Result.Failure("Cannot add null announcements");
            }

            announcements.ForEach(announcement => LaboratoryAnnouncements.Add(announcement));
            return Result.Success();
        }

        public Result AddLaboratoryGradingInfos(List<GradingInfo> gradingInfos)
        {
            if (gradingInfos.Any(info => info == null))
            {
                return Result.Failure("Cannot add null grading infos");
            }

            gradingInfos.ForEach(gradingInfo => LaboratoryGradingInfo.Add(gradingInfo));
            return Result.Success();
        }
    }
}
