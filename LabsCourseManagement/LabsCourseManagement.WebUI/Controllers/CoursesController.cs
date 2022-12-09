using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(courseRepository.GetAll().Result);
        }

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

        [HttpPost]
        public IActionResult Create([FromBody] CreateCourseDto courseDto)
        {
            var professor = professorRepository.GetById(courseDto.ProfessorId);
            if (professor == null)
            {
                return NotFound($"Professor with id {courseDto.ProfessorId} does not exist");
            }

            var course = Course.Create(courseDto.Name);
            if (course.IsSuccess)
            {
                course.Entity.AddProfessors(new List<Professor> { professor.Result });
                courseRepository.Add(course.Entity);
                courseRepository.Save();
                professor.Result.AddCourses(new List<Course> { course.Entity });
                return Created(nameof(Get), course);
            }
            return BadRequest(course.Error);
        }

        [HttpPost("{courseId:guid}/professors")]
        public IActionResult AddProfessorsToCourse(Guid courseId, [FromBody] List<Guid> professorsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null)
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
                if (professor== null)
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

        [HttpPost("{courseId:guid}/students")]
        public IActionResult AddStudentsToCourse(Guid courseId, [FromBody] List<Guid> studentsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null)
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
                if (student == null)
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

        [HttpDelete("{courseId:guid}")]
        public IActionResult Delete(Guid courseId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null)
            {
                return NotFound();
            }
            courseRepository.Delete(course.Result);
            courseRepository.Save();
            return NoContent();
        }
    }
}
