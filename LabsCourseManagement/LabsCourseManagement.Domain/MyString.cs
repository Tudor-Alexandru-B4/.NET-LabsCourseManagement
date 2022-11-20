using LabsCourseManagement.Domain.Helpers;

namespace LabsCourseManagement.Domain
{
    public class MyString
    {
        public Guid Id { get; private set; }
        public string String { get; private set; }

        public static Result<MyString> Create(string String)
        {
            var myString = new MyString()
            {
                Id = Guid.NewGuid(),
                String = String
            };
            return Result<MyString>.Success(myString);
        }

    }
}
