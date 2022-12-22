using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Response;
using LabsCourseManagement.Domain;
using LabsCourseManagement.Domain.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{studentId:guid}")]
        public Task<Student> Get(Guid studentId) =>
            mediator.Send(new GetStudentQuery { Id = studentId });

        [HttpGet]
        public async Task<List<Student>> Get()
        {
            return await mediator.Send(new GetAllStudentsQuery());
        }

        [HttpPost] 
        public async Task<ActionResult<StudentResponse>> Create([FromBody] CreateStudentRequest request) 
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

    }
}
