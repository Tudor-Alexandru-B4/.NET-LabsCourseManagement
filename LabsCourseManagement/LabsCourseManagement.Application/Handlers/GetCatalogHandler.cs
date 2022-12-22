using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Handlers
{
    public class GetCatalogHandler : IRequestHandler<GetCatalogQuery, Catalog>
    {
        private readonly ICatalogRepository catalogRepository;

        public GetCatalogHandler(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        public async Task<Catalog> Handle(GetCatalogQuery request, CancellationToken cancellationToken)
        {
            return await catalogRepository.Get(request.Id);
        }
    }

}