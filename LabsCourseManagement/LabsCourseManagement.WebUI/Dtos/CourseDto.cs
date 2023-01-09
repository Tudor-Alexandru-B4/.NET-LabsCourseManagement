using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public List<ProfessorDto>? Professors { get; set; }
        public List<StudentDto>? Students { get; set; }
        public List<AnnouncementDto>? CourseAnnouncements { get; set; }
        public List<TimeAndPlaceDto>? CourseProgram { get; set; }
        public List<GradingInfoDto>? CourseGradingInfo { get; set; }
        public List<InformationStringDto>? HelpfulMaterials { get; set; }
    }
}
