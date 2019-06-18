using ApiProject.Interfaces;
using ApiProject.Models.Dtos.File;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = ApiProject.Models.File;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiProject.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class FileController : Controller
    {
        private readonly IFileService service;
        private readonly IMapper mapper;


        public FileController(IFileService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilesAsync()
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //create a container 
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

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

            if (blobs is null)
            {
                return NotFound();
            }

            var currentfiles = mapper.Map<List<FileGet>>(files);
            return Ok(currentfiles);


            //BlobContinuationToken blobContinuationToken = null;
            //BlobResultSegment results = null;

            //var files = new List<File>();

            //do
            //{
            //    results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
            //    results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);

            //    // Get the value of the continuation token returned by the listing call.

            //    foreach (var item in results.Results)
            //    {

            //        var file = new File
            //        {
            //            Name = item.Uri.Segments.Last(),
            //            Type = item.Parent.GetHashCode().ToString()
            //        };
            //        files.Add(file);
            //        //Console.WriteLine(item.Uri);
            //    }
            //} while (blobContinuationToken != null);


            //var currentfiles = mapper.Map<List<FileGet>>(files);
            //if (results is null)
            //{
            //    return NotFound();
            //}

            //return Ok(currentfiles);
        }



        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromBody]string localFileName)
        {

            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);

            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //create a container 
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            string sourceFile = null;
            string destinationFile = null;

            string localPath = "D:\\Azure";
            //string localFileName = "test1.txt";
            sourceFile = Path.Combine(localPath, localFileName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            destinationFile = sourceFile;
            await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);
            return Ok();
        }

        ///GET api/<controller>/5
        [HttpGet("upload")]
        public async Task<IActionResult> UploadFile([FromBody] string localFileName)
        {
            try
            {
                var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
                //string storageConnection = CloudConfigurationManager.GetSetting(BlobStorageConnectionString);
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

                string sourceFile = null;
                string localPath = "C:\\Users\\Admin\\Documents\\Files";
                //string localFileName = "test1.txt";
                sourceFile = Path.Combine(localPath, localFileName);

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

                await cloudBlockBlob.UploadFromFileAsync(sourceFile);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteFile([FromBody] string localFileName)
        {
            try
            {
                var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
                //string storageConnection = CloudConfigurationManager.GetSetting(BlobStorageConnectionString);
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filesync");

                //string localFileName = "text1.txt";

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
                if (cloudBlockBlob is null)
                {
                    return NotFound();
                }
                await cloudBlockBlob.DeleteIfExistsAsync();


                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
