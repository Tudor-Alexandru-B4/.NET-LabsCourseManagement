using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILaboratoryRepository laboratoryRepository;

        public StudentsController(IStudentRepository studentRepository, ICourseRepository courseRepository, ILaboratoryRepository laboratoryRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.laboratoryRepository = laboratoryRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(studentRepository.GetAll());
        }

        [HttpGet("{studentId:guid}")]
        public IActionResult Get(Guid studentId)
        {
            var student = studentRepository.Get(studentId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentDto studentDto)
        {
            var student = Student.Create(studentDto.Name, studentDto.Surname, studentDto.Group, studentDto.Year, studentDto.RegistrationNumber, studentDto.PhoneNumber);
            if (student.IsSuccess)
            {
                studentRepository.Add(student.Entity);
                studentRepository.Save();
                return Created(nameof(Get), student);
            }
            return BadRequest(student.Error);
        }

        [HttpPost("{studentId:guid}/courses")]
        public IActionResult AddCoursesToStudent(Guid studentId, [FromBody] List<Guid> coursesIds)
        {
            var student = studentRepository.Get(studentId);
            if (student == null)
            {
                return NotFound($"Student with id {studentId} does not exist");
            }

            if (!coursesIds.Any())
            {
                return BadRequest("Add at least one course");
            }

            var courses = new List<Course>();
            foreach (var courseId in coursesIds)
            {
                var course = courseRepository.Get(courseId);
                if (course == null)
                {
                    return NotFound($"Course with id {courseId} does not exist");
                }
                courses.Add(course);
            }
            student.AddCourse(courses);
            studentRepository.Save();
            return NoContent();
        }

        [HttpPost("{studentId:guid}/laboratories")]
        public IActionResult AddLaboratoriesToStudent(Guid studentId, [FromBody] List<Guid> laboratoriesIds)
        {
            var student = studentRepository.Get(studentId);
            if (student == null)
            {
                return NotFound($"Student with id {studentId} does not exist");
            }

            if (!laboratoriesIds.Any())
            {
                return BadRequest("Add at least one laboratory");
            }

            var laboratories = new List<Laboratory>();
            foreach (var laboratoryId in laboratoriesIds)
            {
                var laboratory = laboratoryRepository.Get(laboratoryId);
                if (laboratory == null)
                {
                    return NotFound($"Laboratory with id {laboratoryId} does not exist");
                }
                laboratories.Add(laboratory);
            }
            student.AddLaboratories(laboratories);
            studentRepository.Save();
            return NoContent();
        }

        [HttpDelete("{studentId:guid}")]
        public IActionResult Delete(Guid studentId)
        {
            var student = studentRepository.Get(studentId);
            if (student == null)
            {
                return NotFound();
            }
            studentRepository.Delete(student);
            studentRepository.Save();
            return NoContent();
        }

        [HttpPost("{studentId:guid}/changeGroup")]
        public IActionResult ChangeStudentGroup(Guid studentId, [FromBody] string newGroup)
        {
            var student = studentRepository.Get(studentId);
            if(student == null)
            {
                return NotFound();
            }
            studentRepository.ChangeGroup(student, newGroup);
            studentRepository.Save();
            return NoContent();
        }

    }
}
