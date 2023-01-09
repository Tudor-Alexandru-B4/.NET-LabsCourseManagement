﻿using LabsCourseManagement.Application.Repositories;
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
        private readonly IAnnouncementRepository announcementRepository;
        private readonly ICatalogRepository catalogRepository;
        private readonly ITimeAndPlaceRepository timeAndPlaceRepository;
        private readonly ILaboratoryRepository laboratoryRepository;
        private readonly IContactRepository contactRepository;
        private readonly IInformationStringRepository informationStringRepository;
        private readonly IGradingInfoRepository gradingInfoRepository;

        public CoursesController(ICourseRepository courseRepository, IProfessorRepository professorRepository,
            IStudentRepository studentRepository, IAnnouncementRepository announcementRepository,
            ICatalogRepository catalogRepository, ITimeAndPlaceRepository timeAndPlaceRepository,
            ILaboratoryRepository laboratoryRepository, IContactRepository contactRepository,
            IInformationStringRepository informationStringRepository, IGradingInfoRepository gradingInfoRepository)
        {
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.studentRepository = studentRepository;
            this.announcementRepository = announcementRepository;
            this.catalogRepository = catalogRepository;
            this.timeAndPlaceRepository = timeAndPlaceRepository;
            this.laboratoryRepository = laboratoryRepository;
            this.contactRepository = contactRepository;
            this.informationStringRepository = informationStringRepository;
            this.gradingInfoRepository = gradingInfoRepository;
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

            //DELETING LABORATORIES
            var courseLaboratories = course.Result.Laboratories;
            foreach (var laboratoryForId in courseLaboratories)
            {
                var laboratoryResult = laboratoryRepository.Get(laboratoryForId.Id);
                var laboratory = laboratoryResult.Result;

                if (laboratory == null || laboratory == null)
                {
                    return NotFound();
                }

                foreach (var announcement in laboratory.LaboratoryAnnouncements)
                {
                    announcementRepository.Delete(announcement);
                }

                timeAndPlaceRepository.Delete(laboratory.LaboratoryTimeAndPlace);
                catalogRepository.Delete(laboratory.LaboratoryCatalog);

                laboratoryRepository.Delete(laboratory);
                laboratoryRepository.Save();
                catalogRepository.Save();
                timeAndPlaceRepository.Save();
                announcementRepository.Save();
            }

            //CONTINUING WITH COURSE
            foreach (var announcement in course.Result.CourseAnnouncements)
            {
                announcementRepository.Delete(announcement);
            }

            foreach (var timeAndPlace in course.Result.CourseProgram)
            {
                timeAndPlaceRepository.Delete(timeAndPlace);
            }

            catalogRepository.Delete(course.Result.CourseCatalog);

            courseRepository.Delete(course.Result);
            courseRepository.Save();
            catalogRepository.Save();
            announcementRepository.Save();
            timeAndPlaceRepository.Save();
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

        [MapToApiVersion("2.0")]
        [HttpPost("{courseId:guid}/announcements")]
        public IActionResult AddAnnouncementsToCourse(Guid courseId, [FromBody] List<CreateAnnouncementDto> announcementsDto)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
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

            course.Result.AddCourseAnnouncements(announcements);
            announcementRepository.Save();
            courseRepository.Save();

            //Sending emails to students
            foreach (var studentForId in course.Result.Students)
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
        [HttpPut("{courseId:guid}/announcements")]
        public IActionResult RemoveAnnouncementsToCourse(Guid courseId, [FromBody] List<Guid> announcementsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
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

            course.Result.RemoveCourseAnnouncements(announcements);
            courseRepository.Save();

            foreach (var announcement in announcements)
            {
                announcementRepository.Delete(announcement);
            }

            announcementRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{courseId:guid}/programs")]
        public IActionResult AddProgramsToCourse(Guid courseId, [FromBody] List<CreateTimeAndPlaceDto> timeAndPlaceDtos)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!timeAndPlaceDtos.Any())
            {
                return BadRequest("Add at least one program");
            }

            var programs = new List<TimeAndPlace>();
            foreach (var timeAndPlace in timeAndPlaceDtos)
            {
                if (timeAndPlace == null || timeAndPlace.DateTime == null || timeAndPlace.Classroom == null)
                {
                    return BadRequest();
                }

                if (timeAndPlaceRepository.Exists(DateTime.Parse(timeAndPlace.DateTime), timeAndPlace.Classroom))
                {
                    return BadRequest($"Room {timeAndPlace.Classroom} is occupied at {timeAndPlace.DateTime}");
                }

                var program = TimeAndPlace.Create(DateTime.Parse(timeAndPlace.DateTime), timeAndPlace.Classroom).Entity;
                if (timeAndPlace == null)
                {
                    return BadRequest("Invalid time or place format");
                }
                timeAndPlaceRepository.Add(program);
                programs.Add(program);
            }

            course.Result.AddCoursePrograms(programs);
            courseRepository.Save();
            timeAndPlaceRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/programs")]
        public IActionResult RemoveProgramsToCourse(Guid courseId, [FromBody] List<Guid> programsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!programsId.Any())
            {
                return BadRequest("Add at least one program");
            }

            var programs = new List<TimeAndPlace>();
            foreach (var programId in programsId)
            {
                var program = timeAndPlaceRepository.Get(programId);

                if (program == null || program.Result == null)
                {
                    return NotFound($"Program with {programId} does not exist");
                }
                programs.Add(program.Result);
            }

            course.Result.RemoveCoursePrograms(programs);
            courseRepository.Save();

            foreach (var program in programs)
            {
                timeAndPlaceRepository.Delete(program);
            }

            timeAndPlaceRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{courseId:guid}/materials")]
        public IActionResult AddMaterialsToCourse(Guid courseId, [FromBody] List<string> materials)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!materials.Any())
            {
                return BadRequest("Add at least one material");
            }

            var helpfulMaterials = new List<InformationString>();
            foreach (var material in materials)
            {
                var helpfulMaterial = InformationString.Create(material);
                if (helpfulMaterial == null || helpfulMaterial.IsFailure)
                {
                    return BadRequest();
                }

                helpfulMaterials.Add(helpfulMaterial.Entity);
                informationStringRepository.Add(helpfulMaterial.Entity);
            }

            course.Result.AddHelpfulMaterials(helpfulMaterials);
            courseRepository.Save();
            informationStringRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/materials")]
        public IActionResult RemoveMaterialsToCourse(Guid courseId, [FromBody] List<Guid> materialsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
            }

            if (!materialsId.Any())
            {
                return BadRequest("Add at least one material");
            }

            var materials = new List<InformationString>();
            foreach (var materialId in materialsId)
            {
                var material = informationStringRepository.GetById(materialId);

                if (material == null || material.Result == null)
                {
                    return NotFound($"Program with {materialId} does not exist");
                }
                materials.Add(material.Result);
            }

            course.Result.RemoveHelpfulMaterials(materials);
            courseRepository.Save();

            foreach (var material in materials)
            {
                informationStringRepository.Delete(material);
            }

            informationStringRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{courseId:guid}/gradings")]
        public IActionResult AddGradingsToCourse(Guid courseId, [FromBody] List<CreateGradingsDto> gradingsDto)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
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

            course.Result.AddCourseGradingInfos(gradings);
            courseRepository.Save();
            gradingInfoRepository.Save();
            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{courseId:guid}/gradings")]
        public IActionResult RemoveGradingsToCourse(Guid courseId, [FromBody] List<Guid> gradingsId)
        {
            var course = courseRepository.Get(courseId);
            if (course == null || course.Result == null)
            {
                return NotFound($"Course with id {courseId} does not exist");
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

            course.Result.RemoveCourseGradingInfos(gradings);
            courseRepository.Save();

            foreach (var grading in gradings)
            {
                gradingInfoRepository.Delete(grading);
            }

            gradingInfoRepository.Save();
            return NoContent();
        }
    }
}
