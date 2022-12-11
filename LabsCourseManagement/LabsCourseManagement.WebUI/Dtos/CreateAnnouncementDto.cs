using FluentValidation;
using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class CreateAnnouncementDto
    {
        public string? Header { get;  set; }
        public string? Text { get;  set; }
    }

    public class AnnouncementValidator : AbstractValidator<CreateAnnouncementDto>
    {
        public AnnouncementValidator()
        {
            RuleFor(x => x.Header).NotEmpty().WithMessage("Please insert a header");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Please insert a text");
        }
    }
}
