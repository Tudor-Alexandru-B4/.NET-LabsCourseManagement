using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LabsCourseManagement.WebUI.Controllers
{
    [Route("v1/api/[controller]")]
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
            return Ok(announcementRepository.GetAll().Result);
        }

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
        [HttpPost("{professorId:guid}")]
        public IActionResult Create([FromBody] CreateAnnouncementDto announcementDto, Guid professorId)
        {
            var writter=professorRepository.GetById(professorId);
            var announcement = Announcement.Create(announcementDto.Header, announcementDto.Text, writter.Result);
            if(announcement.IsSuccess)
            {
                announcementRepository.Add(announcement.Entity);
                announcementRepository.Save();
                return Created(nameof(Get), announcement);
            }
            return BadRequest(announcement.Error);
        }
        [HttpDelete("{announcementId:guid}")]
        public IActionResult Delete(Guid announcementId)
        {
            var announcement = announcementRepository.GetById(announcementId);
            if (announcement == null)
            {
                return NotFound();
            }
            announcementRepository.Delete(announcement.Result);
            announcementRepository.Save();
            return NoContent();
        }
    }
}
