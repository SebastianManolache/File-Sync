using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Interfaces
{
    interface IAzureManager
    {
         CloudBlobContainer ContainerConnectAzure(string container);
    }
}
