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
        private readonly IStudentRepository studentRepository;

        public LaboratoriesController(ILaboratoryRepository laboratoryRepository, 
            ICourseRepository courseRepository, IProfessorRepository professorRepository,
            ITimeAndPlaceRepository timeAndPlaceRepository, IStudentRepository studentRepository)
        {
            this.laboratoryRepository = laboratoryRepository;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.timeAndPlaceRepository = timeAndPlaceRepository;
            this.studentRepository = studentRepository;
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
            if (timeAndPlaceRepository.Exists(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place))
            {
                return BadRequest($"Room {laboratoryDto.Place} is occupied at {laboratoryDto.DateTime}");
            }
            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place).Entity;
            timeAndPlaceRepository.Add(timeAndPlace);
            timeAndPlaceRepository.Save();

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

        [HttpDelete("{laboratoryId:guid}")]
        public IActionResult Delete(Guid laboratoryId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null)
            {
                return NotFound();
            }
            laboratoryRepository.Delete(laboratory);
            laboratoryRepository.Save();
            return NoContent();
        }

        [HttpPost("{laboratoryId:guid}/addStudents")]
        public IActionResult AddStudents(Guid laboratoryId, [FromBody] List<Guid> studentsIds)
        {
            foreach (Guid studentId in studentsIds)
            {
                if (studentRepository.Get(studentId) == null)
                {
                    return NotFound();
                }
            }

            var students = studentsIds.Select(studentRepository.Get).ToList();

            var laboratory = laboratoryRepository.Get(laboratoryId);
            var addStudentsResult = laboratory.AddStudents(students);

            if (addStudentsResult.IsSuccess)
            {
                laboratoryRepository.Save();
                return NoContent();
            }
            return BadRequest(addStudentsResult.Error);
        }

    }
}
