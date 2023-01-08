namespace LabsCourseManagement.Shared.Domain
{
    public class ContactModel
    {
        public ContactModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get;  set; }
        public string? PhoneNumber { get; set; }
        public List<InformationStringModel>? EmailAddresses { get; set; }
    }
}
