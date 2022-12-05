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
            if (phoneNumber == null)
            {
                return Result<Contact>.Failure("Phone number cannot be null");
            }

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
            if (emailAddresses.Any(email => email == null))
            {
                return Result.Failure("Email cannot be null");
            }

            emailAddresses.ForEach(emailAddresses => EmailAddresses.Add(emailAddresses));
            return Result.Success();
        }
        public Result UpdateContact(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    return Result.Failure("Invalid Phone Number");
                }
            }
            PhoneNumber=phoneNumber;

            return Result.Success();
        }

    }
}
