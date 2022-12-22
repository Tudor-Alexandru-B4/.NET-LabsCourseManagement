using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CatalogsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Catalog>> Get()
        {
            return await mediator.Send(new GetAllCatalogsQuery());
        }

        [HttpGet("{catalogId:guid}")]
        public Task<Catalog> Get(Guid catalogId) =>
            mediator.Send(new GetCatalogQuery { Id = catalogId });
    }
}
