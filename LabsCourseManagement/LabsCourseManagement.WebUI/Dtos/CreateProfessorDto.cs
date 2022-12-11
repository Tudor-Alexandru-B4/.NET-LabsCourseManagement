using FluentValidation;
using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateProfessorDto
    {
        public string? Name { get;  set; }
        public string? Surname { get;  set; }
        public string? PhoneNumber { get; set; }
    }

    public class ProfessorValidator : AbstractValidator<CreateProfessorDto>
    {
        public ProfessorValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Please insert a surname");
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^(\+\d{1,3}( )?)?((\(\d{3}\))|\d{3})[- .]?\d{3}[- .]?\d{4}$")
               .WithMessage("Numbers with this format are required: 0742844992 +40 (074)6543210, +995 (987)-654-3210");
        }
    }
}
