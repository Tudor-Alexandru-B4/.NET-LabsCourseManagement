using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Domain;
using MediatR;

namespace LabsCourseManagement.Application.Handlers
{
    public class GetAllCatalogsHandler : IRequestHandler<GetAllCatalogsQuery, List<Catalog>>
    {
        private readonly ICatalogRepository catalogRepository;

        public GetAllCatalogsHandler(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        public async Task<List<Catalog>> Handle(GetAllCatalogsQuery request, CancellationToken cancellationToken)
        {
            return await catalogRepository.GetAll();
        }
    }
}
