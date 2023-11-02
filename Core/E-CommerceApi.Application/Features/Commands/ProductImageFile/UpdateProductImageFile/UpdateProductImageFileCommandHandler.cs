using E_CommerceApi.Application.Abstractions.Storage;
using E_CommerceApi.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using p= E_CommerceApi.Domain.Entities.Common;

namespace E_CommerceApi.Application.Features.Commands.ProductImageFile.UpdateProductImageFile
{
    public class UpdateProductImageFileCommandHandler : IRequestHandler<UpdateProductImageFileCommandRequest, UpdateProductImageFileCommandResponse>
    {
        readonly IStorageService _storageService;
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UpdateProductImageFileCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UpdateProductImageFileCommandResponse> Handle(UpdateProductImageFileCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await
               _storageService.UploadAsync("photo-images", request.Files);
            p.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entities.ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Product = new List<p.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
