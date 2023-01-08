using LabsCourseManagement.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(contactRepository.GetAll().Result);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{contactId:guid}")]
        public IActionResult Get(Guid contactId)
        {
            var contact = contactRepository.Get(contactId).Result;
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
    }
}
