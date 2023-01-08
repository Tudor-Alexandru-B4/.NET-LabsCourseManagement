using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Application.Response;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILaboratoryRepository laboratoryRepository;
        private readonly IContactRepository contactRepository;
        private readonly IInformationStringRepository informationStringRepository;

        public StudentsController(IMediator mediator, IStudentRepository studentRepository,
            ICourseRepository courseRepository, ILaboratoryRepository laboratoryRepository,
            IContactRepository contactRepository, IInformationStringRepository informationStringRepository)
        {
            this.mediator = mediator;
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.laboratoryRepository = laboratoryRepository;
            this.contactRepository = contactRepository;
            this.informationStringRepository = informationStringRepository;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{studentId:guid}")]
        public async Task<IActionResult> Get(Guid studentId)
        {
            var students = await mediator.Send(new GetStudentQuery { Id = studentId });
            return Ok(students);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var student = await mediator.Send(new GetAllStudentsQuery());
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [MapToApiVersion("1.0")]
        [HttpPost] 
        public async Task<ActionResult<StudentResponse>> Create([FromBody] CreateStudentRequest request) 
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{studentId:guid}/courses")]
        public IActionResult AddCoursesToStudent(Guid studentId, [FromBody] List<Guid> coursesIds)
        {
            var student = studentRepository.Get(studentId);
            if (student == null || student.Result == null)
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
                if (course == null || course.Result == null)
                {
                    return NotFound($"Course with id {courseId} does not exist");
                }
                courses.Add(course.Result);
            }
            student.Result.AddCourse(courses);
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{studentId:guid}/laboratories")]
        public IActionResult AddLaboratoriesToStudent(Guid studentId, [FromBody] List<Guid> laboratoriesIds)
        {
            var student = studentRepository.Get(studentId);
            if (student == null || student.Result == null)
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
                if (laboratory == null || laboratory.Result == null)
                {
                    return NotFound($"Laboratory with id {laboratoryId} does not exist");
                }
                laboratories.Add(laboratory.Result);
            }
            student.Result.AddLaboratories(laboratories);
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{studentId:guid}")]
        public IActionResult Delete(Guid studentId)
        {
            var student = studentRepository.Get(studentId);
            if (student == null || student.Result == null)
            {
                return NotFound();
            }
            studentRepository.Delete(student.Result);
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{studentId:guid}/group")]
        public IActionResult ChangeStudentGroup(Guid studentId, [FromBody] string newGroup)
        {
            if(newGroup == null)
            {
                return BadRequest();
            }

            var student = studentRepository.Get(studentId);
            if (student == null || student.Result == null)
            {
                return NotFound();
            }
            student.Result.ChangeGroup(newGroup);
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{studentId:guid}/email")]
        public IActionResult AddEmailToStudent(Guid studentId, [FromBody] string email)
        {
            var student = studentRepository.Get(studentId);
            if (student == null || student.Result == null)
            {
                return NotFound($"Student with id {studentId} does not exist");
            }

            if (email == null)
            {
                return BadRequest("Email cannot be null");
            }

            var emailResult = InformationString.Create(email);
            if (emailResult == null || emailResult.IsFailure)
            {
                return BadRequest();
            }

            var contactStudent = contactRepository.Get(student.Result.ContactInfo.Id);
            if (contactStudent == null || contactStudent.Result == null)
            {
                return BadRequest();
            }

            informationStringRepository.Add(emailResult.Entity);
            informationStringRepository.Save();

            var emailAddResult = contactStudent.Result.AddEmailAddressToList(new List<InformationString>()
            {
                emailResult.Entity
            });

            if (emailAddResult == null || emailAddResult.IsFailure)
            {
                return BadRequest();
            }
            
            contactRepository.Save();
            studentRepository.Save();
            return NoContent();
        }

    }
}
