using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace LabsCourseManagement.WebUI.Dtos
{
    public class EmailSendDto
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void SendEmail()
        {
            if (Recipient == null || Body == null || Subject == null)
            {
                return;
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("gtatutu2001@gmail.com"));
            email.To.Add(MailboxAddress.Parse(Recipient));
            email.Subject = Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = Body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("gtatutu2001@gmail.com", "smujkjtkaqmnhkxy");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
