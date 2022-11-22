using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class InformationString
    {
        public Guid Id { get; private set; }
        public string String { get; private set; }

        public static Result<InformationString> Create(string String)
        {
            var myString = new InformationString()
            {
                Id = Guid.NewGuid(),
                String = String
            };
            return Result<InformationString>.Success(myString);
        }

    }
}
