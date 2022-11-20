namespace LabsCourseManagement.Domain
{
    public class Contact
    {
        public Guid Id { get; private set; }
        public string PhoneNumber { get; private set; }
        public List<MyString> EmailAddresses { get; private set; }

    }
}
