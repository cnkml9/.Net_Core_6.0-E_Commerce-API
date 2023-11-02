using E_CommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandHandler : IRequestHandler<ChangeShowCaseImageCommandRequest, ChangeShowCaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowCaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowCaseImageCommandResponse> Handle(ChangeShowCaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                 .Include(p => p.Product)
                 .SelectMany(p => p.Product, (pif, p) => new
                 {
                     pif,
                     p
                 });


            var data = await query
                 .FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase == true);

            if (data != null)
            {
                data.pif.Showcase = false;
            }
           var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageID));

            if (image != null)
            {
                image.pif.Showcase = true;
            }

           await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
