using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Catalog CourseCatalog { get; private set; }
        public bool IsActive { get; private set; }
        public List<Professor> Professors { get; private set; }
        public List<Laboratory> Laboratorys { get; private set; }
        public List<Student> CourseStudents { get; private set; }
        public List<TimeAndPlace> CourseProgram { get; private set; }
        public List<Announcement> CourseAnnouncements { get; private set; }
        public List<GradingInfo> CourseGradingInfo { get; private set; }
        public List<MyString> HelpfulMaterials { get; private set; }

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
                Laboratorys = new List<Laboratory>(),
                CourseStudents = new List<Student>(),
                CourseProgram = new List<TimeAndPlace>(),
                CourseAnnouncements = new List<Announcement>(),
                CourseGradingInfo = new List<GradingInfo>(),
                HelpfulMaterials = new List<MyString>()
            };
            return Result<Course>.Success(course);
        }

        //public Result AddCourseProfessor(Professor courseProfessor)
        //{
        //    if (courseProfessor == null)
        //    {
        //        return Result.Failure("Professor cannot be null");
        //    }

        //    CourseProfessors.Add(courseProfessor);
        //    return Result.Success();
        //}

        //public Result AddLaboratoryProfessor(Professor laboratoryProfessor)
        //{
        //    if (laboratoryProfessor == null)
        //    {
        //        return Result.Failure("Professor cannot be null");
        //    }

        //    LaboratoryProfessors.Add(laboratoryProfessor);
        //    return Result.Success();
        //}

        public Result AddLaboratories(List<Laboratory> laboratories)
        {
            if (!laboratories.Any())
            {
                return Result.Failure("Laboratories cannot be null");
            }

            laboratories.ForEach(lab => Laboratorys.Add(lab));
            return Result.Success();
        }

        public Result AddCourseStudents(List<Student> students)
        {
            if (!students.Any())
            {
                return Result.Failure("Students cannot be null");
            }

            students.ForEach(student => CourseStudents.Add(student));
            return Result.Success();
        }
        
        public Result AddCoursePrograms(List<TimeAndPlace> timesAndPlaces)
        {
            if (!timesAndPlaces.Any())
            {
                return Result.Failure("TimesAndPlaces cannot be null");
            }

            timesAndPlaces.ForEach(timeAndPlace => CourseProgram.Add(timeAndPlace));
            return Result.Success();
        }

        public Result AddCourseAnnouncements(List<Announcement> announcements)
        {
            if (!announcements.Any())
            {
                return Result.Failure("Announcements cannot be null");
            }

            announcements.ForEach(announcement => CourseAnnouncements.Add(announcement));
            return Result.Success();
        }

        public Result AddCourseGradingInfos(List<GradingInfo> gradingInfos)
        {
            if (!gradingInfos.Any())
            {
                return Result.Failure("GradingInfos cannot be null");
            }

            gradingInfos.ForEach(gradingInfo => CourseGradingInfo.Add(gradingInfo));
            return Result.Success();
        }

        public Result AddHelpfulMaterials(List<MyString> helpfulMaterials)
        {
            if (!helpfulMaterials.Any())
            {
                return Result.Failure("HelpfulMaterials cannot be null");
            }

            helpfulMaterials.ForEach(helpfulMaterial => HelpfulMaterials.Add(helpfulMaterial));
            return Result.Success();
        }

    }
}
