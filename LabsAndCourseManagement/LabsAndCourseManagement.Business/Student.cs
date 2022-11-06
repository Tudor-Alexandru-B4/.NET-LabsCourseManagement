namespace LabsAndCourseManagement.Business

{
    public class Student
    {
        Guid StudentId { get; set; }
        String Name { get; set; }
        String Surname { get; set; }
        Contact ContactInfo { get; set; }
        int Year { get; set; }
        String Group { get; set; }
        Boolean IsActive { get; set; }
        String RegistrationNumber { get; set; }
        List<Course> Courses { get; set; }
        List<Laboratory> Laboratories { get; set; }
    }

}
