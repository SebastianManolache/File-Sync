using ApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProject.Interfaces
{
    public interface IFileService
    {
        List<File> GetFiles();
        Task<File> UploadFileAsync(string fileName);
        Task<bool> DeleteAsync(string fileName);
        File GetByName(string fileName);
        Task<bool> DownloadFileAsync(string fileName);
    }
}
