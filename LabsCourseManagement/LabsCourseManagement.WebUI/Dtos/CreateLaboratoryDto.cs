using FluentValidation;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateLaboratoryDto
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public Guid ProfessorId { get; set; }
        public string DateTime { get; set; }
        public string Place { get; set; }
    }

    public class LaboratoryValidator : AbstractValidator<CreateLaboratoryDto>
    {
        public LaboratoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.CourseId).NotNull().WithMessage("Please insert a valid course id");
            RuleFor(x => x.ProfessorId).NotNull().WithMessage("Please insert a valid professor id");
            RuleFor(x => x.DateTime).NotEmpty().WithMessage("Please insert a date");
            RuleFor(x => x.Place).NotEmpty().WithMessage("Please insert a place");
        }
    }
}
