using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
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

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(professorRepository.GetAll().Result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{professorId:guid}")]
        public IActionResult Get(Guid professorId)
        {
            var professor = professorRepository.GetById(professorId).Result;
            if (professor == null)
            {
                return NotFound();
            }
            return Ok(professor);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateProfessorDto professorDto)
        {
            if (professorDto.Name == null || professorDto.Surname == null || professorDto.PhoneNumber == null)
            {
                return BadRequest();
            }

            var professor = Professor.Create(professorDto.Name, professorDto.Surname, professorDto.PhoneNumber);
            if (professor.IsSuccess && professor.Entity != null)
            {
                professorRepository.Add(professor.Entity);
                professorRepository.Save();
                return Created(nameof(Get), professor);
            }
            return BadRequest(professor.Error);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{professorId:guid}")]
        public IActionResult Delete(Guid professorId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            professorRepository.Delete(professor.Result);
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}/courses")]
        public ActionResult UpdateCourses(Guid professorId, [FromBody] List<Guid> coursesId)
        {
            var courses = new List<Course>();
            var professor = professorRepository.GetById(professorId);
            var professors = new List<Professor>();

            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            foreach (var courseId in coursesId)
            {
                var course = courseRepository.Get(courseId);
                if (course == null || course.Result == null)
                {
                    return NotFound();
                }
                courses.Add(course.Result);
                professors.Add(professor.Result);
                course.Result.AddProfessors(professors);
            }
            professor.Result.AddCourses(courses);
            professorRepository.Save();
            courseRepository.Save();
            return NoContent();

        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}/laboratories")]
        public ActionResult UpdateLaboratories(Guid professorId, [FromBody] List<Guid> laboratoriesId)
        {
            var laboratories = new List<Laboratory>();
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            foreach (var laboratoryId in laboratoriesId)
            {
                var laboratory = laboratoryRepository.Get(laboratoryId);
                if (laboratory == null || laboratory.Result == null)
                {
                    return NotFound();
                }
                laboratories.Add(laboratory.Result);
            }
            professor.Result.AddLaboratories(laboratories);
            professorRepository.Save();
            return NoContent();

        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}/{contactId:guid}/phoneNumber")]
        public ActionResult UpdatePhoneNumber(Guid professorId, Guid contactId, [FromBody] string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return BadRequest();
            }

            var professor = professorRepository.GetById(professorId);
            if(professor == null || professor.Result == null)
            {
                return NotFound();
            }

            var contact = contactRepository.Get(contactId);

            if (contact == null)
            {
                return NotFound();
            }
            professor.Result.UpdatePhoneNumber(phoneNumber);
            professorRepository.Save();
            contactRepository.Save();

            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}/name")]
        public ActionResult UpdateName(Guid professorId, [FromBody] string name)
        {
            if (name == null)
            {
                return BadRequest();
            }
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            professor.Result.UpdateName(name);
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}/surname")]
        public ActionResult UpdateSurname(Guid professorId, [FromBody] string surname)
        {
            if (surname == null)
            {
                return BadRequest();
            }
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            professor.Result.UpdateSurname(surname);
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{professorId:guid}/{courseId:guid}")]
        public IActionResult RemoveCourse(Guid professorId,Guid courseId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            var course = courseRepository.Get(courseId);
            if (course.Result == null)
            {
                return NotFound();
            }
            var result=professor.Result.RemoveCourse(course.Result);
            if(result == null)
            {
                return BadRequest();
            }
            courseRepository.Save();
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{professorId:guid}/{laboratoryId:guid}")]
        public IActionResult RemoveLaboratory(Guid professorId, Guid laboratoryId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory.Result == null)
            {
                return NotFound();
            }
            var result = professor.Result.RemoveLaboratory(laboratory.Result);
            if (result == null)
            {
                return BadRequest();
            }
            laboratoryRepository.Save();
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{professorId:guid}/courses")]
        public ActionResult RemoveCourses(Guid professorId, [FromBody] List<Guid> coursesId)
        {
            var courses = new List<Course>();
            var professor = professorRepository.GetById(professorId);
            var professors = new List<Professor>();

            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            foreach (var courseId in coursesId)
            {
                var course = courseRepository.Get(courseId);
                if (course == null || course.Result == null)
                {
                    return NotFound();
                }
                courses.Add(course.Result);
                course.Result.RemoveProfessors(professors);
                professor.Result.RemoveCourse(course.Result);
            }
            professorRepository.Save();
            courseRepository.Save();
            return NoContent();

        }

        [MapToApiVersion("2.0")]
        [HttpPut("{professorId:guid}/laboratories")]
        public ActionResult RemoveLaboratories(Guid professorId, [FromBody] List<Guid> laboratoriesId)
        {
            var professor = professorRepository.GetById(professorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound();
            }
            foreach (var laboratoryId in laboratoriesId)
            {
                var laboratory = laboratoryRepository.Get(laboratoryId);
                if (laboratory == null || laboratory.Result == null)
                {
                    return NotFound();
                }

                professor.Result.RemoveLaboratory(laboratory.Result);

            }
            professorRepository.Save();
            return NoContent();

        }
    }
}
