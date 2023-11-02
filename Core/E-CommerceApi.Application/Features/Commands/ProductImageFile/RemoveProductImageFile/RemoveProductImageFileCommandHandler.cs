using E_CommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace E_CommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImageFile
{
    public class RemoveProductImageFileCommandHandler : IRequestHandler<RemoveProductImageFileCommandRequest, RemoveProductImageFileCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public RemoveProductImageFileCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<RemoveProductImageFileCommandResponse> Handle(RemoveProductImageFileCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Common.Product? product = await _productReadRepository.Table.Include(p => p.productImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            Domain.Entities.ProductImageFile? productImageFile = product?.productImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.imageId));
            if (productImageFile != null)
            {
                product?.productImageFiles.Remove(productImageFile);
            }
            

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
