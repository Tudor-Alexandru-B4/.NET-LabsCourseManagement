using System.ComponentModel.DataAnnotations;

namespace LabsCourseManagement.UI.Pages.InputClasses
{
    public class LaboratoryInput
    {
        [Required(ErrorMessage = "Please insert a name")]
        public string? Name { get; set; }
        public Guid CourseId { get; set; }
        public Guid ProfessorId { get; set; }
        [Required(ErrorMessage ="Please insert a date")]
        [RegularExpression(@"^(?:(?:31(/|-|.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(/|-|.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(/|-|.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(/|-|.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$", ErrorMessage ="Incorrect format")]
        public string? DateTime { get; set; }
        [Required(ErrorMessage = "Please insert a place")]
        public string? Place { get; set; }
    }
}
