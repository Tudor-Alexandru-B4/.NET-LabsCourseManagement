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
            return Ok(laboratoryRepository.GetAll().Result);
        }

        [HttpGet("{laboratoryId:guid}")]
        public IActionResult Get(Guid laboratoryId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId).Result;
            if (laboratory == null)
            {
                return NotFound();
            }
            return Ok(laboratory);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLaboratoryDto laboratoryDto)
        {
            if (laboratoryDto.DateTime == null || laboratoryDto.Place == null || laboratoryDto.Name == null)
            {
                return BadRequest();
            }

            var laboratoryProfessor = professorRepository.GetById(laboratoryDto.ProfessorId);
            if (laboratoryProfessor == null || laboratoryProfessor.Result== null)
            {
                return NotFound();
            }

            var course = courseRepository.Get(laboratoryDto.CourseId);
            if (course == null || course.Result== null)
            {
                return NotFound();
            }

            if (timeAndPlaceRepository.Exists(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place))
            {
                return BadRequest($"Room {laboratoryDto.Place} is occupied at {laboratoryDto.DateTime}");
            }

            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place).Entity;
            if (timeAndPlace == null)
            {
                return BadRequest("Invalid time or place format");
            }
            timeAndPlaceRepository.Add(timeAndPlace);
            timeAndPlaceRepository.Save();

            var laboratory = Laboratory.Create(laboratoryDto.Name, course.Result, 
                laboratoryProfessor.Result, timeAndPlace);

            if (laboratory.IsSuccess && laboratory.Entity != null)
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
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound();
            }
            laboratoryRepository.Delete(laboratory.Result);
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

            var students=new List<Student>();
            foreach (var studentId in studentsIds)
            {
                var student= studentRepository.Get(studentId).Result;
                if (student != null)
                {
                    students.Add(student);
                }
            }

            var laboratory = laboratoryRepository.Get(laboratoryId);
            if(laboratory == null || laboratory.Result == null)
            {
                return NotFound();
            }

            var addStudentsResult = laboratory.Result.AddStudents(students);

            if (addStudentsResult.IsSuccess)
            {
                laboratoryRepository.Save();
                return NoContent();
            }
            return BadRequest(addStudentsResult.Error);
        }

    }
}
