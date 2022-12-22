using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Queries
{
    public class GetStudentQuery : IRequest<Student>
    {
        public Guid Id { get; set; }
    }
}
