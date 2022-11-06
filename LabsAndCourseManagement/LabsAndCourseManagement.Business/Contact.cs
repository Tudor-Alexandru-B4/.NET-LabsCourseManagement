namespace LabsAndCourseManagement.Business
{
    public class Contact
    {
        public Guid Id { get; private set; }
        public List<string> PhoneNumbers { get; private set; }
        public List<string> EmailAddresses { get; private set; }

    }
}
