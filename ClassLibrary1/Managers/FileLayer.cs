using Data.Interfaces;
using Data.Models;
using Data.Models.Dtos.File;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using File = Data.Models.File;
using ApiProject;
using Microsoft.Data.Entity;
using ApiProject.Context;

namespace Data.Managers
{
    public class FileLayer : IFileLayer
    {

        public List<FileGet> GetFiles()
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

            var list = cloudBlobContainer.ListBlobs(useFlatBlobListing: true);
            var blobs = cloudBlobContainer.ListBlobs().OfType<CloudBlockBlob>().ToList();
            var files = new List<FileGet>();

            foreach (var blob in blobs)
            {
                var file = new FileGet
                {
                    Name = blob.Name,
                    Size = (int)blob.Properties.Length,
                    CreatedAt = DateTime.Parse(blob.Properties.Created.ToString()),
                    Type = blob.Properties.ContentType
                };
                files.Add(file);
            }
            return files;
            //using (var db = new FileDbContext())
            //{
            //    var files = db
            //        .File
            //        .ToListAsync().Result;
            //    return files;
            //}
            //return null;
        }

        public async Task<File> UploadFileAsync(string fileName)
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

            // string sourceFile = null;
            // string localPath = "C:\\Users\\Admin\\Documents\\Files";
            //sourceFile = Path.Combine(localPath, fileName);

            string sourceFile = fileName;
            string localFileName = Path.GetFileName(fileName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            var currentFile = GetByName(localFileName);
            if (currentFile is null)
            {
                await cloudBlockBlob.UploadFromFileAsync(sourceFile);

                var currentFile1 = GetByName(localFileName);

                using (var db = new FileDbContext())
                {
                    await db.File.AddAsync(currentFile1);
                    await db.SaveChangesAsync();
                    return currentFile1;
                }
            }
            return currentFile;
        }
        public File GetByName(string fileName)
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

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

        public async Task<bool> DownloadFileAsync(string localFileName)
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
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

        public async Task<bool> DeleteAsync(string localFileName)
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
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
    }
}
