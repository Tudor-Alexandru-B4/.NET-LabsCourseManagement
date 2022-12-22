using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CatalogsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var catalogs = await mediator.Send(new GetAllCatalogsQuery());
            return Ok(catalogs);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{catalogId:guid}")]
        public async Task<IActionResult> Get(Guid catalogId)
        {
            var catalog = await mediator.Send(new GetCatalogQuery { Id = catalogId });
            if (catalog == null)
            {
                return NotFound();
            }
            return Ok(catalog);
        }
    }
}
