using LabsCourseManagement.Domain;
using System.Collections.Generic;

namespace LabsCourseManagement.UnitTests
{
    public class ContactTests
    {
        private const string phoneNumber = "1234567890";

        [Fact]
        public void When_CreateContact_Then_ShouldReturnContact()
        {
            // Arrange
            // Act
            var result = Contact.Create(phoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.PhoneNumber.Should().Be(phoneNumber);
            result.Entity.EmailAddresses.Should().BeEmpty();
        }

        [Fact]
        public void When_CreateContactWithNullField_Then_ShouldReturnFailure()
        {
            // Arrange
            // Act
            var result = Contact.Create(null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Phone number cannot be null");
        }

        [Fact]
        public void When_AddEmails_Then_ShouldAddEmails()
        {
            // Arrange
            var emails = CreateEmailsSUT();
            var contact = Contact.Create(phoneNumber).Entity;

            // Act
            var addEmailsResult = contact.AddEmailAddressToList(emails);

            // Assert
            addEmailsResult.IsSuccess.Should().BeTrue();
            contact.EmailAddresses.Should().NotBeEmpty();
            contact.EmailAddresses.Should().HaveCount(3);
            contact.EmailAddresses.Should().BeEquivalentTo(emails);
        }

        [Fact]
        public void When_AddNullEmail_Then_ShouldReturnFailure()
        {
            // Arrange
            var contact = Contact.Create(phoneNumber).Entity;
            var emails = CreateEmailsSUT();
            emails.Add(null);

            // Act
            var addEmailsResult = contact.AddEmailAddressToList(emails);

            //Assert
            addEmailsResult.IsFailure.Should().BeTrue();
            addEmailsResult.Error.Should().Be("Email cannot be null");
            contact.EmailAddresses.Should().BeEmpty();
        }

        private static List<InformationString> CreateEmailsSUT()
        {
            var email1 = InformationString.Create("name1@email.com").Entity;
            var email2 = InformationString.Create("name2@email.com").Entity;
            var email3 = InformationString.Create("name3@email.com").Entity;
            var emails = new List<InformationString> { email1, email2, email3 };
            return emails;
        }
    }
}
