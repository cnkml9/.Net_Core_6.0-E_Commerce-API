using E_CommerceApi.Application.Abstractions.Storage;
using E_CommerceApi.Application.Abstractions.Token;
using E_CommerceApi.Infrastructure.enums;
using E_CommerceApi.Infrastructure.Services;
using E_CommerceApi.Infrastructure.Services.Storage;
using E_CommerceApi.Infrastructure.Services.Storage.Azure;
using E_CommerceApi.Infrastructure.Services.Storage.Local;
using E_CommerceApi.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler,TokenHandler>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) 
            where T : Storage,
            IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection serviceCollection,StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;              
            }
            
        }

    }
}
