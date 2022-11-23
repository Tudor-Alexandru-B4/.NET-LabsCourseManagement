namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Year { get; set; }
        public string Group { get; set; }
        public string RegistrationNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}
