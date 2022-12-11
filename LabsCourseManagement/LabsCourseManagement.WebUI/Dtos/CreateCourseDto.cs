using FluentValidation;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateCourseDto
    {
        public string Name { get; set; }
        public Guid ProfessorId { get; set; }
    }

    public class CourseValidator : AbstractValidator<CreateCourseDto>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.ProfessorId).NotNull().WithMessage("Please insert a valid professor id");
        }
    }
}
