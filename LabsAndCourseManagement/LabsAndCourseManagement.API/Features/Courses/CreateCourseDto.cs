using LabsAndCourseManagement.Business;

namespace LabsAndCourseManagement.API.Features.Courses
{
    public class CreateCourseDto
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
    }
}
