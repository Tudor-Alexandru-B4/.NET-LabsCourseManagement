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
    public class LaboratoriesController : ControllerBase
    {
        private readonly ILaboratoryRepository laboratoryRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly ITimeAndPlaceRepository timeAndPlaceRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IAnnouncementRepository announcementRepository;
        private readonly ICatalogRepository catalogRepository;
        private readonly IContactRepository contactRepository;
        private readonly IGradingInfoRepository gradingInfoRepository;

        public LaboratoriesController(ILaboratoryRepository laboratoryRepository, 
            ICourseRepository courseRepository, IProfessorRepository professorRepository,
            ITimeAndPlaceRepository timeAndPlaceRepository, IStudentRepository studentRepository,
            IAnnouncementRepository announcementRepository, ICatalogRepository catalogRepository,
            IContactRepository contactRepository, IGradingInfoRepository gradingInfoRepository)
        {
            this.laboratoryRepository = laboratoryRepository;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.timeAndPlaceRepository = timeAndPlaceRepository;
            this.studentRepository = studentRepository;
            this.announcementRepository = announcementRepository;
            this.catalogRepository = catalogRepository;
            this.contactRepository = contactRepository;
            this.gradingInfoRepository = gradingInfoRepository;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(laboratoryRepository.GetAll().Result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{laboratoryId:guid}")]
        public IActionResult Get(Guid laboratoryId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId).Result;
            if (laboratory == null)
            {
                return NotFound();
            }
            return Ok(laboratory);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateLaboratoryDto laboratoryDto)
        {
            if (laboratoryDto.DateTime == null || laboratoryDto.Place == null || laboratoryDto.Name == null)
            {
                return BadRequest();
            }

            var laboratoryProfessor = professorRepository.GetById(laboratoryDto.ProfessorId);
            if (laboratoryProfessor == null || laboratoryProfessor.Result== null)
            {
                return NotFound();
            }

            var course = courseRepository.Get(laboratoryDto.CourseId);
            if (course == null || course.Result== null)
            {
                return NotFound();
            }

            if (timeAndPlaceRepository.Exists(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place))
            {
                return BadRequest($"Room {laboratoryDto.Place} is occupied at {laboratoryDto.DateTime}");
            }

            var timeAndPlace = TimeAndPlace.Create(DateTime.Parse(laboratoryDto.DateTime), laboratoryDto.Place).Entity;
            if (timeAndPlace == null)
            {
                return BadRequest("Invalid time or place format");
            }
            timeAndPlaceRepository.Add(timeAndPlace);
            timeAndPlaceRepository.Save();

            var laboratory = Laboratory.Create(laboratoryDto.Name, course.Result, 
                laboratoryProfessor.Result, timeAndPlace);

            if (laboratory.IsSuccess && laboratory.Entity != null)
            {
                laboratoryRepository.Add(laboratory.Entity);
                laboratoryRepository.Save();

                course.Result.AddLaboratories(new List<Laboratory> { laboratory.Entity });
                courseRepository.Save();

                laboratoryProfessor.Result.AddLaboratories(new List<Laboratory> { laboratory.Entity });
                professorRepository.Save();

                return Created(nameof(Get), laboratory);
            }
            return BadRequest(laboratory.Error);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{laboratoryId:guid}")]
        public IActionResult Delete(Guid laboratoryId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound();
            }

            foreach (var announcement in laboratory.Result.LaboratoryAnnouncements)
            {
                announcementRepository.Delete(announcement);
            }

            timeAndPlaceRepository.Delete(laboratory.Result.LaboratoryTimeAndPlace);
            catalogRepository.Delete(laboratory.Result.LaboratoryCatalog);

            laboratoryRepository.Delete(laboratory.Result);
            laboratoryRepository.Save();
            catalogRepository.Save();
            timeAndPlaceRepository.Save();
            announcementRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("{laboratoryId:guid}/addStudents")]
        public IActionResult AddStudents(Guid laboratoryId, [FromBody] List<Guid> studentsIds)
        {
            foreach (Guid studentId in studentsIds)
            {
                if (studentRepository.Get(studentId) == null)
                {
                    return NotFound();
                }
            }

            var students=new List<Student>();
            foreach (var studentId in studentsIds)
            {
                var student= studentRepository.Get(studentId).Result;
                if (student != null)
                {
                    students.Add(student);
                }
            }

            var laboratory = laboratoryRepository.Get(laboratoryId);
            if(laboratory == null || laboratory.Result == null)
            {
                return NotFound();
            }

            var addStudentsResult = laboratory.Result.AddStudents(students);

            if (addStudentsResult.IsSuccess)
            {
                laboratoryRepository.Save();

                foreach (var student in students)
                {
                    student.AddLaboratories(new List<Laboratory> { laboratory.Result });
                }
                studentRepository.Save();

                return NoContent();
            }
            return BadRequest(addStudentsResult.Error);
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/students")]
        public IActionResult RemoveStudents(Guid laboratoryId, [FromBody] List<Guid> studentsIds)
        {
            foreach (Guid studentId in studentsIds)
            {
                if (studentRepository.Get(studentId) == null)
                {
                    return NotFound();
                }
            }

            var students = new List<Student>();
            foreach (var studentId in studentsIds)
            {
                var student = studentRepository.Get(studentId).Result;
                if (student != null)
                {
                    students.Add(student);
                }
            }

            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound();
            }

            var removeStudentsResult = laboratory.Result.RemoveStudents(students);

            if (removeStudentsResult.IsSuccess)
            {
                laboratoryRepository.Save();
                return NoContent();
            }
            return BadRequest(removeStudentsResult.Error);
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/name")]
        public IActionResult UpdateName(Guid laboratoryId, [FromBody] string name)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            var updateResult = laboratory.Result.UpdateName(name);
            if (updateResult.IsFailure)
            {
                return BadRequest();
            }

            courseRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/active")]
        public IActionResult UpdateActiveStatus(Guid laboratoryId, [FromBody] bool activeStatus)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            var updateResult = laboratory.Result.UpdateActiveStatus(activeStatus);
            if (updateResult.IsFailure)
            {
                return BadRequest();
            }

            courseRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/professor")]
        public IActionResult UpdateProfessor(Guid laboratoryId, [FromBody] Guid newProfessorId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            var newProfessor = professorRepository.GetById(newProfessorId);
            if (newProfessor == null || newProfessor.Result == null)
            {
                return NotFound($"Professor with id {newProfessorId} does not exist");
            }

            var oldProfessor = professorRepository.GetById(laboratory.Result.LaboratoryProfessor.Id);
            if (newProfessor == null || newProfessor.Result == null)
            {
                return BadRequest();
            }

            var updateResult = laboratory.Result.UpdateProfessor(newProfessor.Result);
            if (updateResult == null || updateResult.IsFailure)
            {
                return BadRequest();
            }

            laboratoryRepository.Save();
            professorRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{laboratoryId:guid}/announcements")]
        public IActionResult AddAnnouncementsToLab(Guid laboratoryId, [FromBody] List<CreateAnnouncementDto> announcementsDto)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            if (!announcementsDto.Any())
            {
                return BadRequest("Add at least one professor");
            }

            var announcements = new List<Announcement>();
            foreach (var announcement in announcementsDto)
            {
                var professor = professorRepository.GetById(announcement.ProfessorId);
                if (professor == null || professor.Result == null)
                {
                    return NotFound($"Professor with id {announcement.ProfessorId} does not exist");
                }

                var announcementResult =
                    Announcement.Create(announcement.Header, announcement.Text, professor.Result);
                if (announcementResult == null || announcementResult.Entity == null)
                {
                    return BadRequest();
                }
                announcementRepository.Add(announcementResult.Entity);
                announcements.Add(announcementResult.Entity);
            }

            laboratory.Result.AddLaboratoryAnnouncements(announcements);
            announcementRepository.Save();
            laboratoryRepository.Save();

            //Sending emails to students
            foreach (var studentForId in laboratory.Result.LaboratoryStudents)
            {
                var student = studentRepository.Get(studentForId.StudentId);

                if (student == null || student.Result == null)
                {
                    return BadRequest();
                }

                var studentContactInfo = contactRepository.Get(student.Result.ContactInfo.Id);

                if (studentContactInfo == null || studentContactInfo.Result == null || studentContactInfo.Result.EmailAddresses == null)
                {
                    return BadRequest();
                }

                foreach (var emailAdress in studentContactInfo.Result.EmailAddresses)
                {
                    if (emailAdress != null)
                    {
                        foreach (var announcement in announcements)
                        {
                            var email = new EmailSendDto()
                            {
                                Recipient = emailAdress.String,
                                Subject = announcement.Header,
                                Body = announcement.Text
                            };
                            email.SendEmail();
                        }
                    }
                }
            }
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/announcements")]
        public IActionResult RemoveAnnouncementsToLab(Guid laboratoryId, [FromBody] List<Guid> announcementsId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            if (!announcementsId.Any())
            {
                return BadRequest("Add at least one announcement");
            }

            var announcements = new List<Announcement>();
            foreach (var announcementId in announcementsId)
            {
                var announcement = announcementRepository.GetById(announcementId);

                if (announcement == null || announcement.Result == null)
                {
                    return NotFound($"Announcement with {announcementId} does not exist");
                }
                announcements.Add(announcement.Result);
            }

            laboratory.Result.RemoveLaboratoryAnnouncements(announcements);
            laboratoryRepository.Save();

            foreach (var announcement in announcements)
            {
                announcementRepository.Delete(announcement);
            }

            announcementRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{laboratoryId:guid}/gradings")]
        public IActionResult AddGradingsToLab(Guid laboratoryId, [FromBody] List<CreateGradingsDto> gradingsDto)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            if (!gradingsDto.Any())
            {
                return BadRequest("Add at least one grading");
            }

            var gradings = new List<GradingInfo>();
            foreach (var gradingDto in gradingsDto)
            {
                ExaminationType examinationType;
                if (!ExaminationType.TryParse(gradingDto.ExaminationType, true, out examinationType))
                {
                    return BadRequest();
                }

                if (timeAndPlaceRepository.Exists(DateTime.Parse(gradingDto.DateTime), gradingDto.Classroom))
                {
                    return BadRequest();
                }

                var grading = GradingInfo.Create(examinationType, gradingDto.MinGrade, gradingDto.MaxGrade,
                    gradingDto.IsMandatory, gradingDto.Description, TimeAndPlace.Create(DateTime.Parse(gradingDto.DateTime), gradingDto.Classroom).Entity);
                if (grading == null || grading.IsFailure)
                {
                    return BadRequest();
                }

                gradings.Add(grading.Entity);
                gradingInfoRepository.Add(grading.Entity);
            }

            laboratory.Result.AddLaboratoryGradingInfos(gradings);
            laboratoryRepository.Save();
            gradingInfoRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{laboratoryId:guid}/gradings")]
        public IActionResult RemoveGradingsToLab(Guid laboratoryId, [FromBody] List<Guid> gradingsId)
        {
            var laboratory = laboratoryRepository.Get(laboratoryId);
            if (laboratory == null || laboratory.Result == null)
            {
                return NotFound($"Laboratory with id {laboratoryId} does not exist");
            }

            if (!gradingsId.Any())
            {
                return BadRequest("Add at least one grading");
            }

            var gradings = new List<GradingInfo>();
            foreach (var gradingId in gradingsId)
            {
                var grading = gradingInfoRepository.Get(gradingId);

                if (grading == null || grading.Result == null)
                {
                    return NotFound($"Grading with {gradingId} does not exist");
                }
                gradings.Add(grading.Result);
            }

            laboratory.Result.RemoveLaboratoryGradingInfos(gradings);
            laboratoryRepository.Save();

            foreach (var grading in gradings)
            {
                gradingInfoRepository.Delete(grading);
            }

            gradingInfoRepository.Save();
            return NoContent();
        }

    }
}
