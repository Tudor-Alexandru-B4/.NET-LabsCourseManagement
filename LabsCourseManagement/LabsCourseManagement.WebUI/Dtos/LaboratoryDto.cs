using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class LaboratoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<StudentDto> LaboratoryStudents { get; set; }
    }
}
