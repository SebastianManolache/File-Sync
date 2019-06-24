using Data.Models;
using Data.Models.Dtos.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Interfaces
{
    public interface IFileLayer
    {
        List<FileGet> GetFiles();
        Task<File> UploadFileAsync(string fileName);
        Task<bool> DownloadFileAsync(string localFileName);
        File GetByName(string fileName);
        Task<bool> DeleteAsync(string localFileName);
    }
}
