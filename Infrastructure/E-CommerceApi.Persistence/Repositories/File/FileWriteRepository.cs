﻿using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<E_CommerceApi.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(E_CommerceApiDbContext context) : base(context)
        {
        }
    }
}
