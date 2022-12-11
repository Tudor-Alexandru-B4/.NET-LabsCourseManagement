using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public Catalog? CourseCatalog { get; private set; }
        public bool IsActive { get; private set; }
        public List<Professor>? Professors { get; private set; }
        public List<Laboratory>? Laboratories { get; private set; }
        public List<Student>? Students { get; private set; }
        public List<TimeAndPlace>? CourseProgram { get; private set; }
        public List<Announcement>? CourseAnnouncements { get; private set; }
        public List<GradingInfo>? CourseGradingInfo { get; private set; }
        public List<InformationString>? HelpfulMaterials { get; private set; }

        public static Result<Course> Create(string name)
        {
            if(name == null)
            {
                return Result<Course>.Failure("Name cannot be null");
            }

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Name = name,
                CourseCatalog = Catalog.Create().Entity,
                IsActive = true,
                Professors = new List<Professor>(),
                Laboratories = new List<Laboratory>(),
                Students = new List<Student>(),
                CourseProgram = new List<TimeAndPlace>(),
                CourseAnnouncements = new List<Announcement>(),
                CourseGradingInfo = new List<GradingInfo>(),
                HelpfulMaterials = new List<InformationString>()
            };
            return Result<Course>.Success(course);
        }

        public Result AddProfessors(List<Professor> professors)
        {
            if (professors.Any(professor => professor == null))
            {
                return Result.Failure("Professors cannot be null");
            }

            professors.ForEach(professor => Professors?.Add(professor));
            return Result.Success();
        }

        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (laboratories.Any(laboratory => laboratory == null))
            {
                return Result.Failure("Laboratories cannot be null");
            }

            laboratories.ForEach(lab => Laboratories?.Add(lab));
            return Result.Success();
        }

        public Result AddCourseStudents(List<Student> students)
        {
            if (students.Any(student => student == null))
            {
                return Result.Failure("Students cannot be null");
            }

            students.ForEach(student => Students?.Add(student));
            return Result.Success();
        }
        
        public Result AddCoursePrograms(List<TimeAndPlace> timesAndPlaces)
        {
            if (timesAndPlaces.Any(timeAndPlace => timeAndPlace == null))
            {
                return Result.Failure("TimesAndPlaces cannot be null");
            }

            timesAndPlaces.ForEach(timeAndPlace => CourseProgram?.Add(timeAndPlace));
            return Result.Success();
        }

        public Result AddCourseAnnouncements(List<Announcement> announcements)
        {
            if (announcements.Any(announcement => announcement == null))
            {
                return Result.Failure("Announcements cannot be null");
            }

            announcements.ForEach(announcement => CourseAnnouncements?.Add(announcement));
            return Result.Success();
        }

        public Result AddCourseGradingInfos(List<GradingInfo> gradingInfos)
        {
            if (gradingInfos.Any(gradingInfo => gradingInfo == null))
            {
                return Result.Failure("GradingInfos cannot be null");
            }

            gradingInfos.ForEach(gradingInfo => CourseGradingInfo?.Add(gradingInfo));
            return Result.Success();
        }

        public Result AddHelpfulMaterials(List<InformationString> helpfulMaterials)
        {
            if (helpfulMaterials.Any(helpfulMaterial => helpfulMaterial == null))
            {
                return Result.Failure("HelpfulMaterials cannot be null");
            }

            helpfulMaterials.ForEach(helpfulMaterial => HelpfulMaterials?.Add(helpfulMaterial));
            return Result.Success();
        }

    }
}
