
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Globalization;
using System.IO;

namespace ApiProject
{
    public static class AzureOperations
    {

        #region ConfigParams
        public static string tenantId;
        public static string applicationId;
        public static string clientSecret;
        #endregion

        public static void UploadFile(AzureOperationHelper azureOperationHelper)
        {
            CloudBlobContainer blobContainer = CreateCloudBlobContainer(tenantId, applicationId,
                                               clientSecret, azureOperationHelper.storageAccountName, azureOperationHelper.containerName,
                                               azureOperationHelper.storageEndPoint);
            blobContainer.CreateIfNotExistsAsync();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(azureOperationHelper.blobName);
            blob.UploadFromFileAsync(azureOperationHelper.srcPath);
        }

        public static void DownloadFile(AzureOperationHelper azureOperationHelper)
        {
            CloudBlobContainer blobContainer = CreateCloudBlobContainer(tenantId, applicationId,
                                               clientSecret, azureOperationHelper.storageAccountName, azureOperationHelper.containerName,
                                               azureOperationHelper.storageEndPoint);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(azureOperationHelper.blobName);
            blob.DownloadToFileAsync(azureOperationHelper.destinationPath, FileMode.OpenOrCreate);
        }

        private static CloudBlobContainer CreateCloudBlobContainer(string tenantId, string applicationId, string clientSecret, string storageAccountName,
            string containerName, string storageEndPoint)
        {
            string accessToken = GetUserOAuthToken(tenantId, applicationId, clientSecret);
            TokenCredential tokenCredential = new TokenCredential(accessToken);
            StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageAccountName, storageEndPoint, useHttps: true);
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            return blobContainer;
        }

        static string GetUserOAuthToken(string tenantId, string applicationId, string clientSecret)
        {
            const string ResourceId = "https://storage.azure.com/";
            const string AuthInstance = "https://login.microsoftonline.com/{0}/";

            string authority = string.Format(CultureInfo.InvariantCulture, AuthInstance, tenantId);
            AuthenticationContext authContext = new AuthenticationContext(authority);

            var clientCred = new ClientCredential(applicationId, clientSecret);
            AuthenticationResult result = authContext.AcquireTokenAsync(
                                                ResourceId,
                                                clientCred
                                                ).Result;
            return result.AccessToken;
        }
    }
}
