using ApiProject.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = ApiProject.Models.File;

namespace ApiProject.Services
{
    public class FileService : IFileService
    {
        private readonly IAzureManager manager;

        

        public async Task<bool> DeleteAsync(string localFileName)
        {
            //var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = manager.ContainerConnectAzure("filesync");
           
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            if (cloudBlockBlob is null)
            {
                return false;
            }

            await cloudBlockBlob.DeleteIfExistsAsync();

            using (var db = new FileDbContext())
            {
                var currentFile = await db.File.FirstOrDefaultAsync(file => file.Name == localFileName);
                if (currentFile is null)
                {
                    return false;
                }

                db.File.Remove(currentFile);
                await db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> DownloadFileAsync(string localFileName)
        {
            //var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            var cloudBlobContainer = manager.ContainerConnectAzure("filesync");
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            string sourceFile = null;
            string destinationFile = null;
            string localPath = "D:\\Azure";

            sourceFile = Path.Combine(localPath, localFileName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
            destinationFile = sourceFile;

            await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);

            return true;
        }

        public File GetByName(string fileName)
        {
            //var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

            var list = cloudBlobContainer.ListBlobs(useFlatBlobListing: true);
            var blobs = cloudBlobContainer.ListBlobs().OfType<CloudBlockBlob>().ToList();
            var files = new List<File>();

            foreach (var blob in blobs)
            {
                if (blob.Name == fileName)
                {
                    return new File
                    {
                        Name = blob.Name,
                        Size = blob.Properties.Length,
                        CreatedAt = DateTime.Parse(blob.Properties.Created.ToString()),
                        Type = blob.Properties.ContentType,
                        UpdatedAt = DateTime.Parse(blob.Properties.LastModified.ToString())
                    };
                }
            }
            return null;
        }

        public List<File> GetFiles()
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

           // var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

            var list = cloudBlobContainer.ListBlobs(useFlatBlobListing: true);
            var blobs = cloudBlobContainer.ListBlobs().OfType<CloudBlockBlob>().ToList();
            var files = new List<File>();

            foreach (var blob in blobs)
            {
                var file = new File
                {
                    Name = blob.Name,
                    Size = blob.Properties.Length,
                    CreatedAt = DateTime.Parse(blob.Properties.Created.ToString()),
                    Type = blob.Properties.ContentType
                };
                files.Add(file);
            }
            return files;
        }

        public async Task<File> UploadFileAsync(string localFileName)
        {
            //var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

            string sourceFile = null;
            string localPath = "C:\\Users\\Admin\\Documents\\Files";
            sourceFile = Path.Combine(localPath, localFileName);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            await cloudBlockBlob.UploadFromFileAsync(sourceFile);
            var currentFile = GetByName(localFileName);
           
            using (var db = new FileDbContext())
            {
                await db.File.AddAsync(currentFile);
                await db.SaveChangesAsync();
                return currentFile;
            }
        }
    }
}
