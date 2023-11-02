using E_CommerceApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Domain.Entities
{
    public class ProductImageFile:File
    {
        public bool Showcase { get; set; }
        public ICollection<Product>?  Product { get; set; }
    }
}
