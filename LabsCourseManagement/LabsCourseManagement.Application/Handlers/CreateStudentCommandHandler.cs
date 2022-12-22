using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Application.Response;
using MediatR;

namespace LabsCourseManagement.Domain.Handlers
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentRequest, StudentResponse>
    {
        private readonly IStudentRepository studentRepository;

        public CreateStudentCommandHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<StudentResponse> Handle(CreateStudentRequest student, CancellationToken cancellationToken)
        {
            if (student.Name == null || student.Surname == null || student.Group == null || student.RegistrationNumber == null || student.PhoneNumber == null)
            {
                throw new ArgumentNullException("There is a null field");
            }

            var newStudent = Student.Create(student.Name, student.Surname, student.Group, student.Year, student.RegistrationNumber, student.PhoneNumber);
            if (newStudent.IsSuccess && newStudent.Entity != null)
            {
                await studentRepository.Add(newStudent.Entity);
                studentRepository.Save();
                return null;
            }
            else
            {
                throw new ArgumentNullException("Eroare");
            }

        }
    }
}
