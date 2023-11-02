using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImageFile
{
    public class RemoveProductImageFileCommandRequest: IRequest<RemoveProductImageFileCommandResponse>  
    {
        public string Id { get; set; }
        public string? imageId { get; set; }
    }
}
