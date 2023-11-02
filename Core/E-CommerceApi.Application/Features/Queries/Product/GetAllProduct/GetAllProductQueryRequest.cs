using E_CommerceApi.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Queries.Product.GetAllProduct
{
    //IRequest Mediatr kütüphanesinden türetilmiştir. GetAllProductRequest in request olduğunu 
    //GetAllProductQueryResponse'nin response olduğunu handler'a bildirmiş oluyoruz.
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
    {
        //public Pagination pagination { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
