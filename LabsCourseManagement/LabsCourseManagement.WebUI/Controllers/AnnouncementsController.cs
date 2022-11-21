using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(announcementRepository.GetAll());
        }

        [HttpGet("{announcementId:guid}")]
        public IActionResult Get(Guid announcementId)
        {
            var announcement = announcementRepository.GetById(announcementId);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }
        [HttpPost("{professorId:guid}")]
        public IActionResult Create([FromBody] CreateAnnouncementDto announcementDto, Guid professorId)
        {
            var writter=professorRepository.GetById(professorId);
            var announcement = Announcement.Create(announcementDto.Header, announcementDto.Text, writter);
            if(announcement.IsSuccess)
            {
                announcementRepository.Add(announcement.Entity);
                announcementRepository.Save();
                return Created(nameof(Get), announcement);
            }
            return BadRequest(announcement.Error);
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] Guid announcementId)
        {
            var announcement = announcementRepository.GetById(announcementId);
            if (announcement == null)
            {
                return NotFound();
            }
            announcementRepository.Delete(announcement);
            announcementRepository.Save();
            return NoContent();
        }
    }
}
