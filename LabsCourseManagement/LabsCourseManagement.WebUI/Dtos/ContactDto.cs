using LabsCourseManagement.Domain;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string? PhoneNumber { get; set; }
        public List<InformationStringDto>? EmailAddresses { get; set; }
    }
}
