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
        private readonly IProfessorRepository professorRepository;
        private readonly ITimeAndPlaceRepository timeAndPlaceRepository;

        public LaboratoriesController(ILaboratoryRepository laboratoryRepository, 
            ICourseRepository courseRepository, IProfessorRepository professorRepository,
            ITimeAndPlaceRepository timeAndPlaceRepository)
        {
            this.laboratoryRepository = laboratoryRepository;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.timeAndPlaceRepository = timeAndPlaceRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(laboratoryRepository.GetAll());
        }

        [HttpGet("{laboratoryId:guid}")]
        public IActionResult Get(Guid laboratoryId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null)
            {
                return NotFound();
            }
            return Ok(laboratory);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLaboratoryDto laboratoryDto)
        {
            if (courseRepository.Get(laboratoryDto.CourseId) == null)
            {
                return NotFound();
            }

            var laboratoryProfessor = professorRepository.GetById(laboratoryDto.ProfessorId);
            var course = courseRepository.Get(laboratoryDto.CourseId);
            var timeAndPlace = timeAndPlaceRepository.Get(laboratoryDto.TimeAndPlaceId);

            var laboratory = Laboratory.Create(laboratoryDto.Name, course, 
                laboratoryProfessor, timeAndPlace);

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
