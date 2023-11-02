using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_CommerceApi.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage :Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);

        }   

        public async Task DeleteAsync(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
           BlobClient blobClient= _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string ContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            return _blobContainerClient.GetBlobs().Select(b=>b.Name).ToList();
        }

        public bool HasFile(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
               return _blobContainerClient.GetBlobs().Any(b=>b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string ContainerName, IFormFileCollection files)
        {
            _blobContainerClient =  _blobServiceClient.GetBlobContainerClient(ContainerName);
            //container yoksa oluştur
            await _blobContainerClient.CreateIfNotExistsAsync();
            //erişim izni veriyoruz
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List <(string fileName, string pathOrContainerName)> datas = new();

            foreach (IFormFile file in files)
            {
              string fileNewName =  await FileRenameAsync(ContainerName, file.FileName, HasFile);

                BlobClient blobClient =  _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());

                datas.Add((fileNewName, $"{ContainerName}/{fileNewName}"));
            }
            return datas;
        }
    }
}
