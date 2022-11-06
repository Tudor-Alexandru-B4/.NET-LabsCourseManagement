namespace LabsAndCourseManagement.Business
{
    public class Professor
    {
        Guid Id { get; set; }
        String Name { get; set; }
        String Surname { get; set; }
        Contact ContactInfo { get; set; }
        Boolean IsActive { get; set; }
        List<Course> Courses { get; set; }
        List<Laboratory> Laboratories { get; set; }

    }
}