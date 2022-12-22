using LabsCourseManagement.Application.Queries;
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
        public async Task<List<Catalog>> Get()
        {
            return await mediator.Send(new GetAllCatalogsQuery());
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{catalogId:guid}")]
        public Task<Catalog> Get(Guid catalogId) =>
            mediator.Send(new GetCatalogQuery { Id = catalogId });
    }
}
