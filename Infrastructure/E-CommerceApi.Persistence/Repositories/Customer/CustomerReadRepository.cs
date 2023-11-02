﻿using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Domain.Entities.Common;
using E_CommerceApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Repositories
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(E_CommerceApiDbContext context) : base(context)
        {
        }
    }
}
