using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<E_CommerceApi.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(E_CommerceApiDbContext context) : base(context)
        {
        }
    }
}
