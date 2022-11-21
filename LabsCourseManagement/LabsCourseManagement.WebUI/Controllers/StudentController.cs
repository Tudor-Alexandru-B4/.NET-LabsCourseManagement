using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
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
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentDto studentDto )
        {
            var student = Student.Create(studentDto.Name, studentDto.Surname, studentDto.Group, studentDto.Year, studentDto.RegistrationNumber);
            if (student.IsSuccess)
            {
                studentRepository.Add(student.Entity);
                studentRepository.Save();
                return Created(nameof(Get), student);
            }
            return BadRequest(student.Error);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Guid studentId)
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
    }
}
