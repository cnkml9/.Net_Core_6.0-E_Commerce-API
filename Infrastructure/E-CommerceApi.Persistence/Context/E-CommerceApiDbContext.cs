using E_CommerceApi.Domain.Entities;
using E_CommerceApi.Domain.Entities.Common;
using E_CommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Context
{
    public class E_CommerceApiDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public E_CommerceApiDbContext(DbContextOptions options) : base(options)
        {
             
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasKey(b => b.Id);

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Order>(b => b.Id);

            base.OnModelCreating(builder);
        }


        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        //{
        //    //changeTracker - Entityler üzerinden yapılan değişikliklerin yada yeni eklenen verinin yakalanmasını sağlayan
        //    //proportydir. Track üzerinden verileri yakalayip elde etemizi sağlar.

        //    var datas = ChangeTracker.Entries<BaseEntity>();

        //    foreach (var item in datas)
        //    {
        //        _ = item.State switch
        //        {
        //            EntityState.Added => item.Entity.CreateDate = DateTime.UtcNow,
        //            EntityState.Modified => item.Entity.UpdatedDate = DateTime.UtcNow
        //        };
        //    }


        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ChangeTracker - Entityler üzerinden yapılan değişikliklerin veya yeni eklenen verinin yakalanmasını sağlayan propertydir.
            // Track üzerinden verileri yakalayıp elde etmemizi sağlar.

            var datas = ChangeTracker.Entries<BaseEntity>();

            var currentTime = DateTime.UtcNow;

            foreach (var item in datas)
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.CreateDate = currentTime;
                    item.Entity.UpdatedDate = currentTime;
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedDate = currentTime;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
