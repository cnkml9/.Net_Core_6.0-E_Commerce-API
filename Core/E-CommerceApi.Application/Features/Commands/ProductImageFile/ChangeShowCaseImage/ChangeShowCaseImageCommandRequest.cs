using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandRequest:IRequest<ChangeShowCaseImageCommandResponse>
    {
        public string ImageID { get; set; }
        public string ProductId { get; set; }
    }
}
