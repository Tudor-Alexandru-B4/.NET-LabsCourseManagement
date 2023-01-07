using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class EmailSenderController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult SendEmail([FromBody] EmailSendDto information)
        {
            if (information == null || information.Recipient == null || information.Body == null || information.Subject == null)
            {
                return BadRequest();
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("gtatutu2001@gmail.com"));
            email.To.Add(MailboxAddress.Parse(information.Recipient));
            email.Subject = information.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = information.Body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("gtatutu2001@gmail.com", "smujkjtkaqmnhkxy");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
