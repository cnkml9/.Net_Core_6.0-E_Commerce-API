using E_CommerceApi.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using E_CommerceApi.Persistence.Conretes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceApi.Persistence.Context;
using Microsoft.Extensions.Configuration;
using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Persistence.Repositories;
using E_CommerceApi.Domain.Entities.Identity;
using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Persistence.Services;
using E_CommerceApi.Application.Abstractions.Services.Authentications;

namespace E_CommerceApi.Persistence
{
    public static class ServiceRegistration
    {
        

        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();

            services.AddDbContext<E_CommerceApiDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<E_CommerceApiDbContext>();

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>(); 
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>(); 
            services.AddScoped<IOrderReadRepository, OrderReadRepository>(); 
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>(); 
            services.AddScoped<IProductReadRepository, ProductReadRepository>(); 
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();

            services.AddScoped<IUserService,UserServices>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication,AuthService>();

            //basket
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            
            
        } 
    }
}
