using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Domain.Entities.Common
{
    public  class Order: BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public Basket Basket { get; set; }
        public ICollection<Domain.Entities.Common.Product> Products { get; set; }
        public Customer Customer { get; set; }
    }
}
