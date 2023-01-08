using LabsCourseManagement.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class InformationStringsController : ControllerBase
    {
        private readonly IInformationStringRepository informationStringRepository;

        public InformationStringsController(IInformationStringRepository informationStringRepository)
        {
            this.informationStringRepository = informationStringRepository;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(informationStringRepository.GetAll().Result);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{informationStringId:guid}")]
        public IActionResult Get(Guid informationStringId)
        {
            var contact = informationStringRepository.GetById(informationStringId).Result;
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
    }
}
