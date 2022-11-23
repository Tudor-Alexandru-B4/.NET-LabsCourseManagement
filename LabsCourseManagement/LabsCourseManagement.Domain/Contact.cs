using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class Contact
    {
        public Guid Id { get; private set; }
        public string PhoneNumber { get; private set; }
        public List<InformationString> EmailAddresses { get; private set; }

        public static Result<Contact> Create(string phoneNumber)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                PhoneNumber = phoneNumber,
                EmailAddresses = new List<InformationString>()
            };
            return Result<Contact>.Success(contact);
        }

        public Result AddEmailAddressToList(List<InformationString> emailAddresses)
        {
            if (emailAddresses == null)
            {
                return Result.Failure("Email cannot be null");
            }
            emailAddresses.ForEach(emailAddresses => EmailAddresses.Add(emailAddresses));
            return Result.Success();
        }

    }
}
