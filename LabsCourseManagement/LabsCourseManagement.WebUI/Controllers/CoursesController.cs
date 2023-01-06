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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IStudentRepository studentRepository;

        public CoursesController(ICourseRepository courseRepository, IProfessorRepository professorRepository, IStudentRepository studentRepository)
        {
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.studentRepository = studentRepository;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(courseRepository.GetAll().Result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{courseId:guid}")]
        public IActionResult Get(Guid courseId)
        {
            var course = courseRepository.Get(courseId).Result;
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateCourseDto courseDto)
        {
            if(courseDto.Name == null)
            {
                return BadRequest();
            }

            var professor = professorRepository.GetById(courseDto.ProfessorId);
            if (professor == null || professor.Result == null)
            {
                return NotFound($"Professor with id {courseDto.ProfessorId} does not exist");
            }

            var course = Course.Create(courseDto.Name);
            if (course.IsSuccess && course.Entity != null)
            {
                course.Entity.AddProfessors(new List<Professor> { professor.Result });
                courseRepository.Add(course.Entity);
                courseRepository.Save();
                professor.Result.AddCourses(new List<Course> { course.Entity });
                professorRepository.Save();
                return Created(nameof(Get), course);
            }
            return BadRequest(course.Error);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{courseId:guid}/professors")]
        public IActionResult AddProfessorsToCourse(Guid courseId, [FromBody] List<Guid> professorsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!professorsId.Any())
            {
                return BadRequest("Add at least one professor");
            }

            var professors = new List<Professor>();
            foreach(var professorId in professorsId)
            {
                var professor = professorRepository.GetById(professorId);
                if (professor== null || professor.Result == null)
                {
                    return NotFound($"Professor with id {professorId} does not exist");
                }
                professor.Result.AddCourses(new List<Course> { course.Result });
                professors.Add(professor.Result);
            }
            
            course.Result.AddProfessors(professors);
            courseRepository.Save();
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{courseId:guid}/students")]
        public IActionResult AddStudentsToCourse(Guid courseId, [FromBody] List<Guid> studentsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!studentsId.Any())
            {
                return BadRequest("Add at least one student");
            }

            var students = new List<Student>();
            foreach (var studentId in studentsId)
            {
                var student = studentRepository.Get(studentId);
                if (student == null || student.Result == null)
                {
                    return NotFound($"Student with {studentId} does not exist");
                }
                student.Result.AddCourse(new List<Course>() { course.Result });
                students.Add(student.Result);
            }

            course.Result.AddCourseStudents(students);
            courseRepository.Save();
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{courseId:guid}")]
        public IActionResult Delete(Guid courseId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound();
            }
            courseRepository.Delete(course.Result);
            courseRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/professors")]
        public IActionResult RemoveProfessorsFromCourse(Guid courseId, [FromBody] List<Guid> professorsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!professorsId.Any())
            {
                return BadRequest("Add at least one professor");
            }

            var professors = new List<Professor>();
            foreach (var professorId in professorsId)
            {
                var professor = professorRepository.GetById(professorId);
                if (professor == null || professor.Result == null)
                {
                    return NotFound($"Professor with id {professorId} does not exist");
                }
                professor.Result.RemoveCourse(course.Result);
                professors.Add(professor.Result);
            }

            course.Result.RemoveProfessors(professors);
            courseRepository.Save();
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/students")]
        public IActionResult RemoveStudentsFromCourse(Guid courseId, [FromBody] List<Guid> studentsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!studentsId.Any())
            {
                return BadRequest("Add at least one student");
            }

            var students = new List<Student>();
            foreach (var studentId in studentsId)
            {
                var student = studentRepository.Get(studentId);
                if (student == null || student.Result == null)
                {
                    return NotFound($"Student with {studentId} does not exist");
                }
                students.Add(student.Result);
            }

            course.Result.RemoveCourseStudents(students);
            courseRepository.Save();
            studentRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/name")]
        public IActionResult UpdateName(Guid courseId, [FromBody] string name)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            var updateResult = course.Result.UpdateName(name);
            if (updateResult.IsFailure)
            {
                return BadRequest();
            }

            courseRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/active")]
        public IActionResult UpdateActiveStatus(Guid courseId, [FromBody] bool activeStatus)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            var updateResult = course.Result.UpdateActiveStatus(activeStatus);
            if (updateResult.IsFailure)
            {
                return BadRequest();
            }

            courseRepository.Save();
            return NoContent();
        }
    }
}
