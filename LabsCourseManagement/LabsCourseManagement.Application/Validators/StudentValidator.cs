using FluentValidation;
using LabsCourseManagement.Application.Queries;

namespace LabsCourseManagement.Application.Validators
{
    public class StudentValidator : AbstractValidator<CreateStudentRequest>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Please insert a surname");
            RuleFor(x => x.Year).GreaterThan(0).WithMessage("Please insert a year that is greater than 0");
            RuleFor(x => x.Group).NotEmpty().WithMessage("Please insert a group");
            RuleFor(x => x.RegistrationNumber).NotEmpty().WithMessage("Please insert your registration number");
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^(\+\d{1,3}( )?)?((\(\d{3}\))|\d{3})[- .]?\d{3}[- .]?\d{4}$")
                .WithMessage("Numbers with this format are required: 0742844992 +40 (074)6543210, +995 (987)-654-3210");
        }
    }
}
