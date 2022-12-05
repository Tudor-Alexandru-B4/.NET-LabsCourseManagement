
using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Shared.Domain
{
    public class ContactModel
    {
        public ContactModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get;  set; }
    }
}
