using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Queries
{
    public class GetCatalogQuery : IRequest<Catalog>
    {
        public Guid Id { get; set; }
    }
}
