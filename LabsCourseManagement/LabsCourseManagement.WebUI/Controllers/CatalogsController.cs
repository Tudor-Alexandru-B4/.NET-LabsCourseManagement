using LabsCourseManagement.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogRepository catalogRepository;

        public CatalogsController(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(catalogRepository.GetAll().Result);
        }

        [HttpGet("{catalogId:guid}")]
        public IActionResult Get(Guid catalogId)
        {
            var catalog = catalogRepository.Get(catalogId).Result;
            if(catalog == null)
            {
                return NotFound();
            }
            return Ok(catalog);
        }
    }
}
