using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Domain.Entities;
using E_CommerceApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Repositories
{
    public class BasketItemReadRepository : ReadRepository<BasketItem>,IBasketItemReadRepository
    {
        public BasketItemReadRepository(E_CommerceApiDbContext context) : base(context)
        {
        }
    }
}
