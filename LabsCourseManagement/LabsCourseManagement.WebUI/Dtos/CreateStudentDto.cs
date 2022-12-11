using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateStudentDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Year { get; set; }
        public string? Group { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class StudentValidator : AbstractValidator<CreateStudentDto>
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
