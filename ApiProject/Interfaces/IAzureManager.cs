using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    interface IAzureManager
    {
         CloudBlobContainer ContainerConnectAzure(string container);
    }
}
