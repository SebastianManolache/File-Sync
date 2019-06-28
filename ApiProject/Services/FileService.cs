using ApiProject.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Data.Models.File;
using ApiProject.Context;
using Utils;

namespace ApiProject.Services
{
    public class FileService : IFileService
    {
        public async Task<bool> DeleteAsync(string localFileName)
        {
            var BlobStorageConnectionString = Encryption.Decrypt("aYIzA9ShsZfHNTAcAEVGED7IJ+txhw6qz/rYLOxWF2zsdnxCvrrm0OZCEjKowtIj9G0fxyAywUqAhOVA8epDGiy0WOOTOeE9EI2vsTDpTu6sWjtH1w+w62nZTE+cmEDbn8M+kNjfU35V2DHDuxhmW+gXsVl6owNqeuO/lISvLOnej2rtCM4h7laKVCojyWdSq608zBTDonIKoio63HW2bY7FtC7Ok7t/4NNZV0TK+K/0r1/s6aPvkA==", true);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

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

        public async Task<bool> DownloadFileAsync(string localFileName)
        {
            var BlobStorageConnectionString = Encryption.Decrypt("aYIzA9ShsZfHNTAcAEVGED7IJ+txhw6qz/rYLOxWF2zsdnxCvrrm0OZCEjKowtIj9G0fxyAywUqAhOVA8epDGiy0WOOTOeE9EI2vsTDpTu6sWjtH1w+w62nZTE+cmEDbn8M+kNjfU35V2DHDuxhmW+gXsVl6owNqeuO/lISvLOnej2rtCM4h7laKVCojyWdSq608zBTDonIKoio63HW2bY7FtC7Ok7t/4NNZV0TK+K/0r1/s6aPvkA==", true);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            // var cloudBlobContainer = manager.ContainerConnectAzure("filesync");
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
            var BlobStorageConnectionString = Encryption.Decrypt("aYIzA9ShsZfHNTAcAEVGED7IJ+txhw6qz/rYLOxWF2zsdnxCvrrm0OZCEjKowtIj9G0fxyAywUqAhOVA8epDGiy0WOOTOeE9EI2vsTDpTu6sWjtH1w+w62nZTE+cmEDbn8M+kNjfU35V2DHDuxhmW+gXsVl6owNqeuO/lISvLOnej2rtCM4h7laKVCojyWdSq608zBTDonIKoio63HW2bY7FtC7Ok7t/4NNZV0TK+K/0r1/s6aPvkA==", true);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            //var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

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
            var BlobStorageConnectionString = Encryption.Decrypt("aYIzA9ShsZfHNTAcAEVGED7IJ+txhw6qz/rYLOxWF2zsdnxCvrrm0OZCEjKowtIj9G0fxyAywUqAhOVA8epDGiy0WOOTOeE9EI2vsTDpTu6sWjtH1w+w62nZTE+cmEDbn8M+kNjfU35V2DHDuxhmW+gXsVl6owNqeuO/lISvLOnej2rtCM4h7laKVCojyWdSq608zBTDonIKoio63HW2bY7FtC7Ok7t/4NNZV0TK+K/0r1/s6aPvkA==", true);

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

        public async Task<File> UploadFileAsync(string sourceFile)
        {
            var BlobStorageConnectionString = Encryption.Decrypt("aYIzA9ShsZfHNTAcAEVGED7IJ+txhw6qz/rYLOxWF2zsdnxCvrrm0OZCEjKowtIj9G0fxyAywUqAhOVA8epDGiy0WOOTOeE9EI2vsTDpTu6sWjtH1w+w62nZTE+cmEDbn8M+kNjfU35V2DHDuxhmW+gXsVl6owNqeuO/lISvLOnej2rtCM4h7laKVCojyWdSq608zBTDonIKoio63HW2bY7FtC7Ok7t/4NNZV0TK+K/0r1/s6aPvkA==",true);
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");
            //  var cloudBlobContainer = manager.ContainerConnectAzure("filesync");

            //string sourceFile = null;
            //string localPath = "C:\\Users\\Admin\\Documents\\Files";
            //sourceFile = Path.Combine(localPath, localFileName);
            string localFileName = Path.GetFileName(sourceFile);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            var currentFile = GetByName(localFileName);
            if (currentFile is null)
            {
                await cloudBlockBlob.UploadFromFileAsync(sourceFile);
                currentFile = GetByName(localFileName);

                using (var db = new FileDbContext())
                {
                    await db.File.AddAsync(currentFile);
                    await db.SaveChangesAsync();
                    return currentFile;
                }
            }
            return null;

        }
    }
}
