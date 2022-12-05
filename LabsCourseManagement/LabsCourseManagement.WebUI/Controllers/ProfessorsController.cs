using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly IProfessorRepository professorRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILaboratoryRepository laboratoryRepository;
        private readonly IContactRepository contactRepository;

        public ProfessorsController(IProfessorRepository professorRepository, ICourseRepository courseRepository, ILaboratoryRepository laboratoryRepository, IContactRepository contactRepository)
        {
            this.professorRepository = professorRepository;
            this.courseRepository = courseRepository;
            this.laboratoryRepository = laboratoryRepository;
            this.contactRepository = contactRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(professorRepository.GetAll());
        }
        [HttpGet("{professorId:guid}")]
        public IActionResult Get(Guid professorId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null)
            {
                return NotFound();
            }
            return Ok(professor);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProfessorDto professorDto)
        {
            var professor = Professor.Create(professorDto.Name, professorDto.Surname, professorDto.PhoneNumber);
            if (professor.IsSuccess)
            {
                professorRepository.Add(professor.Entity);
                professorRepository.Save();
                return Created(nameof(Get), professor);
            }
            return BadRequest(professor.Error);
        }

        [HttpDelete("{professorId:guid}")]
        public IActionResult Delete(Guid professorId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null)
            {
                return NotFound();
            }
            professorRepository.Delete(professor);
            professorRepository.Save();
            return NoContent();
        }
        [HttpPost("{professorId:guid}/courses")]
        public ActionResult UpdateCourses(Guid professorId, [FromBody] List<Guid> coursesId)
        {
            var courses = new List<Course>();
            var professor = professorRepository.GetById(professorId);
            var professors = new List<Professor>();

            if (professor == null)
            {
                return NotFound();
            }
            foreach (var courseId in coursesId)
            {
                var course = courseRepository.Get(courseId);
                if (course == null)
                {
                    return NotFound();
                }
                courses.Add(course);
                professors.Add(professor);
                course.AddProfessors(professors);
            }
            professor.AddCourses(courses);
            professorRepository.Save();
            courseRepository.Save();
            return NoContent();

        }
        [HttpPost("{professorId:guid}/laboratories")]
        public ActionResult UpdateLaboratories(Guid professorId, [FromBody] List<Guid> laboratoriesId)
        {
            var laboratories = new List<Laboratory>();
            var professor = professorRepository.GetById(professorId);
            if (professor == null)
            {
                return NotFound();
            }
            foreach (var laboratoryId in laboratoriesId)
            {
                var laboratory = laboratoryRepository.Get(laboratoryId);
                if (laboratory == null)
                {
                    return NotFound();
                }
                laboratories.Add(laboratory);

            }
            professor.AddLaboratories(laboratories);
            professorRepository.Save();
            return NoContent();

        }
        [HttpPost("{professorId:guid}/{contactId:guid}/phoneNumber")]
        public ActionResult UpdatePhoneNumber(Guid professorId, Guid contactId, [FromBody] string phoneNumber)
        {
            var professor = professorRepository.GetById(professorId);
            var contact = contactRepository.Get(contactId);

            if (professor == null || contact == null)
            {
                return NotFound();
            }
            professor.UpdatePhoneNumber(phoneNumber);
            professorRepository.Save();
            contactRepository.Save();

            return NoContent();
        }

    }
}
