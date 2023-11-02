using E_CommerceApi.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<E_CommerceApiDbContext>
    {
        
        public E_CommerceApiDbContext CreateDbContext(string[] args)
        {
           
            DbContextOptionsBuilder <E_CommerceApiDbContext > DbContextOptionsBuilder = new();
            DbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(DbContextOptionsBuilder.Options);
        }

        
    }
}
