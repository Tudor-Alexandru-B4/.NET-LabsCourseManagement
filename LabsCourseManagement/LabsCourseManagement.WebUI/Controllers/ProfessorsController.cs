using LabsCourseManagement.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
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
