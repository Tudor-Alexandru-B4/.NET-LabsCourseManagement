using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{studentId:guid}")]
        public async Task<IActionResult> Get(Guid studentId)
        {
            var students = await mediator.Send(new GetStudentQuery { Id = studentId });
            return Ok(students);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var student = await mediator.Send(new GetAllStudentsQuery());
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [MapToApiVersion("1.0")]
        [HttpPost] 
        public async Task<ActionResult<StudentResponse>> Create([FromBody] CreateStudentRequest request) 
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

    }
}
