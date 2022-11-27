using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class LaboratoriesController : ControllerBase
    {
        private readonly ILaboratoryRepository laboratoryRepository;
        private readonly ICourseRepository courseRepository;

        public LaboratoriesController(ILaboratoryRepository laboratoryRepository, ICourseRepository courseRepository)
        {
            this.laboratoryRepository = laboratoryRepository;
            this.courseRepository = courseRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(laboratoryRepository.GetAll());
        }

        [HttpGet("{laboratoryId:guid}")]
        public IActionResult Get(Guid laboratoryId)
        {
            var course = laboratoryRepository.Get(laboratoryId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLaboratoryDto laboratoryDto)
        {
            if (courseRepository.Get(laboratoryDto.CourseId) == null)
            {
                return NotFound();
            }

            var laboratory = Laboratory.Create(laboratoryDto.Name, laboratoryDto.CourseId);

            if (laboratory.IsSuccess)
            {
                laboratoryRepository.Add(laboratory.Entity);
                laboratoryRepository.Save();
                return Created(nameof(Get), laboratory);
            }
            return BadRequest(laboratory.Error);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Guid courseId)
        {
            var course = laboratoryRepository.Get(courseId);
            if (course == null)
            {
                return NotFound();
            }
            laboratoryRepository.Delete(course);
            laboratoryRepository.Save();
            return NoContent();
        }
    }
}
