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

        public Result AddLaboratory(Laboratory laboratory)
        {
            if (laboratory == null)
            {
                return Result.Failure("Laboratory cannot be null");
            }

            Laboratorys.Add(laboratory);
            return Result.Success();
        }

        public Result AddCourseStudent(Student student)
        {
            if (student == null)
            {
                return Result.Failure("Student cannot be null");
            }

            CourseStudents.Add(student);
            return Result.Success();
        }
        
        public Result AddCourseProgram(TimeAndPlace timeAndPlace)
        {
            if (timeAndPlace == null)
            {
                return Result.Failure("TimeAndPlace cannot be null");
            }

            CourseProgram.Add(timeAndPlace);
            return Result.Success();
        }

        public Result AddCourseAnnouncement(Announcement announcement)
        {
            if (announcement == null)
            {
                return Result.Failure("Announcement cannot be null");
            }

            CourseAnnouncements.Add(announcement);
            return Result.Success();
        }

        public Result AddCourseGradingInfo(GradingInfo gradingInfo)
        {
            if (gradingInfo == null)
            {
                return Result.Failure("GradingInfo cannot be null");
            }

            CourseGradingInfo.Add(gradingInfo);
            return Result.Success();
        }

        public Result AddHelpfulMaterial(MyString helpfulMaterial)
        {
            if (helpfulMaterial == null)
            {
                return Result.Failure("HelpfulMaterial cannot be null");
            }

            HelpfulMaterials.Add(helpfulMaterial);
            return Result.Success();
        }

    }
}
