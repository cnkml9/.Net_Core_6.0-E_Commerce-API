using E_CommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Common.Product? product = await _productReadRepository.Table.Include(p => p.productImageFiles)
                 .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.id));

            return  product?.productImageFiles.Select(p => new GetProductImagesQueryResponse
            {
                Path= $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                FileName=p.FileName,
                Id=p.Id
            }).ToList();
        }
    }
}
