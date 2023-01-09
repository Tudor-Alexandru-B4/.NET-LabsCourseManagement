using System.ComponentModel.DataAnnotations;

namespace LabsCourseManagement.UI.Pages.InputClasses
{
    public class CourseInput
    {
        [Required(ErrorMessage = "Please insert a name")]
        public string? Name { get; set; }
        public Guid ProfessorId { get; set; }
    }
}
