using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Queries
{
    public class GetAllCatalogsQuery:IRequest<List<Catalog>>
    {
    }
}
