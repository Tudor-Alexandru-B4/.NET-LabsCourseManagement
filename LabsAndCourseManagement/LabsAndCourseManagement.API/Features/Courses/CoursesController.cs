using LabsAndCourseManagement.Business;
using Microsoft.AspNetCore.Mvc;

namespace LabsAndCourseManagement.API.Features.Courses
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(courseRepository.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCourseDto courseDto)
        {
            var course = Course.Create(courseDto.Name);
            if (course.IsSuccess)
            {
                courseRepository.Add(course.Entity);
                return Created(nameof(Get), course);
            }
            return BadRequest(course.Error);
        }
    }
}
