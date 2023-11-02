using E_CommerceApi.Application.Repositories;
using MediatR;
using p = E_CommerceApi.Domain.Entities.Common;

namespace E_CommerceApi.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;

        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            p.Product product =   await _productReadRepository.GetByIdAsync(request.Id, false);
            
            return new()
            {
                Name = product.Name,
                Price = product.Price,
                Stock=product.Stock
            };
        }
    }
}
