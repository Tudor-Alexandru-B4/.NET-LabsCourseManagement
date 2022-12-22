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
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementRepository announcementRepository;
        private readonly IProfessorRepository professorRepository;

        public AnnouncementsController( IAnnouncementRepository announcementRepository, IProfessorRepository professorRepository)
        {
            this.announcementRepository = announcementRepository;
            this.professorRepository = professorRepository;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(announcementRepository.GetAll().Result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{announcementId:guid}")]
        public IActionResult Get(Guid announcementId)
        {
            var announcement = announcementRepository.GetById(announcementId).Result;
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{professorId:guid}")]
        public IActionResult Create([FromBody] CreateAnnouncementDto announcementDto, Guid professorId)
        {
            if (announcementDto.Header == null || announcementDto.Text == null)
            {
                return BadRequest();
            }
            var writter=professorRepository.GetById(professorId);
            if (writter == null || writter.Result == null)
            {
                return NotFound();
            }
            var announcement = Announcement.Create(announcementDto.Header, announcementDto.Text, writter.Result);
            if(announcement.IsSuccess && announcement.Entity != null)
            {
                announcementRepository.Add(announcement.Entity);
                announcementRepository.Save();
                return Created(nameof(Get), announcement);
            }
            return BadRequest(announcement.Error);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{announcementId:guid}")]
        public IActionResult Delete(Guid announcementId)
        {
            var announcement = announcementRepository.GetById(announcementId);
            if (announcement == null || announcement.Result == null)
            {
                return NotFound();
            }
            announcementRepository.Delete(announcement.Result);
            announcementRepository.Save();
            return NoContent();
        }
    }
}
