using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Handlers
{
    public class GetStudentHandler : IRequestHandler<GetStudentQuery, Student>
    {
        private readonly IStudentRepository studentRepository;
        public GetStudentHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<Student> Handle(GetStudentQuery request, CancellationToken cancellationToken = default)
        {
            return await studentRepository.Get(request.Id);
        }
    }
    
}
