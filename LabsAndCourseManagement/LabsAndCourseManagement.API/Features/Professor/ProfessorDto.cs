using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.Laboratories
{
    public class ProfessorDto
    {
        public string Name { get;  set; }
        public string Surname { get;  set; }
        public Contact ContactInfo { get;  set; }
        public bool IsActive { get;  set; }
        public List<Course> Courses { get;  set; }
        public List<Laboratory> Laboratories { get;  set; }
    }
}
