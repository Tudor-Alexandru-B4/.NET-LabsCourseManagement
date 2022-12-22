using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Queries
{
    public class GetAllStudentsQuery : IRequest<List<Student>>
    {
    }
}
