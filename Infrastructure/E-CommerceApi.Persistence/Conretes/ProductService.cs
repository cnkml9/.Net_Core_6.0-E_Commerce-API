﻿using E_CommerceApi.Application.Abstractions;
using E_CommerceApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Conretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts()
            =>new() { 
                new() {  Id = Guid.NewGuid(), Name ="Product-1" , Price=100, Stock=5},
                new() { Id = Guid.NewGuid(), Name = " Product-2", Price=150,Stock=5},
                new()  { Id = Guid.NewGuid(), Name = " Product-3", Price = 200, Stock = 5 }
            };   //target type özelliği
    }
}
