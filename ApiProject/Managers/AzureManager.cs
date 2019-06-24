using ApiProject.Interfaces;
using Data.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ApiProject.Managers
{
    public class AzureManager:IAzureManager
    {
        public CloudBlobContainer ContainerConnectAzure(string container)
        {
            var BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dotnetsa;AccountKey=nNurpFaIPeFZKxPIInKZo/3yPnSOxZCZOxDwnjTv/6trnkox5VcRzHrcqXK6CQo1/uhWeN7MP9Mrn+unxzNofA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            return cloudBlobContainer;
        }
    }
}
