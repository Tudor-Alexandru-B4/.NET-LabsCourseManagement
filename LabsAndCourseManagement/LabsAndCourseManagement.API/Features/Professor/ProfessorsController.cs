using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabsAndCourseManagement.API.Features.Laboratories
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly IProfessorRepository professorRepository;

        public ProfessorsController(IProfessorRepository professorRepository)
        {
            this.professorRepository = professorRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(professorRepository.GetAll());
        }
    }
}
