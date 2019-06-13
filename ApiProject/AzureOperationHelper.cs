using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject
{
    public class AzureOperationHelper
    {
        public string srcPath { get; set; }
        public string destinationPath { get; set; }
        public string storageAccountName { get; set; }
        public string containerName { get; set; }
        public string storageEndPoint { get; set; }
        public string blobName { get; set; }
    }
}

